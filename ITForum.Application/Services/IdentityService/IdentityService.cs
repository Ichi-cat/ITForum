using ITForum.Application.Common.Exceptions;
using ITForum.Application.Common.ViewModels;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ITForum.Application.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IAuthDbContext _authContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public IdentityService(UserManager<ItForumUser> userManager,
            IConfiguration configuration, IEmailService emailService, IAuthDbContext authContext, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _authContext = authContext;
            _tokenValidationParameters = tokenValidationParameters;
        }
        
        #region Reqister
        public async Task<TokenVm> CreateUser(BaseUserInfoModel userInfo, string password)
        {
            var user = await CreateDefaultUser(userInfo, password);

            return await CreateJwtTokenAsync(user);
        }
        public async Task<TokenVm> CreateUserWithProvider(UserLoginInfo providerInfo, BaseUserInfoModel userInfo)
        {
            var user = await CreateDefaultUser(userInfo);
            IdentityResult result = await _userManager.AddLoginAsync(user, providerInfo);
            if (!result.Succeeded) throw new AuthenticationError(result.Errors);
            return await CreateJwtTokenAsync(user);
        }
        private async Task<ItForumUser> CreateDefaultUser(BaseUserInfoModel userInfo, string? password = null)
        {
            if (await _userManager.FindByNameAsync(userInfo.UserName) != null)
                throw new AuthenticationError(new[] { "User is already exists" });
            ItForumUser user = new ItForumUser
            {
                Email = userInfo.Email,
                UserName = userInfo.UserName
            };
            user.EmailConfirmed = userInfo.IsEmailConfirmed;
            IdentityResult result;
            if (password == null)
                result = await _userManager.CreateAsync(user);
            else
                result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) throw new AuthenticationError(result.Errors);
            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return user;
        }
        #endregion

        #region Login
        public async Task<TokenVm> Login(BaseUserInfoModel userInfo, string password)
        {
            var user = await _userManager.FindByNameAsync(userInfo.UserName);
            user ??= await _userManager.FindByEmailAsync(userInfo.Email);
            if (user == null) throw new AuthenticationError(new[] { "User is not exist" });
            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new AuthenticationError(new[] { "Password is not right" });
            return await CreateJwtTokenAsync(user);
        }
        public async Task<TokenVm> Login(string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
            if (user == null) throw new AuthenticationError(new[] { "User is not exist" });
            return await CreateJwtTokenAsync(user);
        }
        #endregion

        #region Operations with User
        public async Task BanUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
        }
        public async Task ResetPassword(string token, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new AuthenticationError(new[] { "User is not found" });
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded) throw new AuthenticationError(result.Errors);
        }
        public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                await _userManager.AddPasswordAsync(user, newPassword);
            }
            else
            {
                if (user == null) throw new AuthenticationError(new[] { "User is not found" });
                await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            }
        }
        public async Task ChangeEmail(string email, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new AuthenticationError(new[] { "User is not found" });
            await _userManager.SetEmailAsync(user, email);
        }
        #endregion

        #region private methods
        private ClaimsPrincipal? GetPrincipal(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;
                return principal;
            }
            catch { return null; }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
        private async Task<List<Claim>> GenerateClaimsToUser(ItForumUser user)
        {
            var claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            });
            return claims;
        }
        #endregion

        #region Tokens
        private async Task<TokenVm> CreateJwtTokenAsync(ItForumUser user)
        {
            var claims = await GenerateClaimsToUser(user);
            var jwt = new JwtSecurityToken(
                    issuer: _configuration["AuthOptions:Issuer"],
                    audience: _configuration["AuthOptions:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.Parse(_configuration["AuthOptions:AccessTokenLifeTime"])),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["AuthOptions:Key"])),
                        SecurityAlgorithms.HmacSha256)
                    );
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid(),
                ExpiresDate = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration["AuthOptions:RefreshTokenLifeTime"])),
                UserId = user.Id
            };
            _authContext.RefreshTokens.RemoveRange(_authContext.RefreshTokens.Where(rToken => rToken.UserId == refreshToken.UserId));
            await _authContext.RefreshTokens.AddAsync(refreshToken);
            await _authContext.SaveChangesAsync();
            return new TokenVm { AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt), RefreshToken = refreshToken.Token, Expiration = jwt.ValidTo };
        }

        public async Task<TokenVm> RefreshToken(Guid refreshToken, string accessToken)
        {
            var principal = GetPrincipal(accessToken);

            if (principal == null) throw new AuthenticationError(new[] { "Invalid token" });
            var UserId = principal.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
            var user = await _userManager.FindByIdAsync(UserId);
            var storedRefreshToken = await _authContext.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshToken);
            if (storedRefreshToken == null) throw new AuthenticationError(new[] { "Invalid token" });
            if (storedRefreshToken.UserId != user.Id) throw new AuthenticationError(new[] { "Invalid token" });
            if (storedRefreshToken.ExpiresDate < DateTime.UtcNow) throw new AuthenticationError(new[] { "Token expired" });
            await _authContext.SaveChangesAsync();

            //generate new token
            return await CreateJwtTokenAsync(user);
        }
        public async Task SendToken(string email, Uri redirectUri)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new AuthenticationError(new[] { "User not exists" });
            if (!user.EmailConfirmed) throw new AuthenticationError(new[] { "Email is not confirmed. You can't reset password" });
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            IDictionary<string, string?> query = new Dictionary<string, string?>() { { "token", token }, { "email", user.Email } };
            var redirect_uri = new Uri(QueryHelpers.AddQueryString(redirectUri.AbsoluteUri, query));

            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Html/ResetPassword.html"));

            string data = File.ReadAllText(path).Replace("linkedUrl", HtmlEncoder.Default.Encode(redirect_uri.ToString()));
            await _emailService.SendAsync(email, "Reset password", data, isHtml: true);
        }
        #endregion
    }
}
