using ITForum.Application.Common.Exceptions;
using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
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
        public IdentityService(UserManager<ItForumUser> userManager,
            IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<JwtSecurityToken> CreateUser(BaseUserInfoModel userInfo, string password)
        {
            var user = await CreateDefaultUser(userInfo, password);

            var claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return CreateJwtToken(user.UserName, user.Email, user.Id.ToString(), claims);
        }

        public async Task<JwtSecurityToken> CreateUserWithProvider(UserLoginInfo providerInfo, BaseUserInfoModel userInfo)
        {
            var user = await CreateDefaultUser(userInfo);
            IdentityResult result = await _userManager.AddLoginAsync(user, providerInfo);
            if (!result.Succeeded) throw new AuthenticationError(result.Errors);
            var claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return CreateJwtToken(user.UserName, user.Email, user.Id.ToString(), claims);
        }
        public async Task<JwtSecurityToken> Login(BaseUserInfoModel userInfo, string password)
        {
            var user = await _userManager.FindByNameAsync(userInfo.UserName);
            user ??= await _userManager.FindByEmailAsync(userInfo.Email);
            if (user == null) throw new AuthenticationError(new[] { "User is not exist" });
            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new AuthenticationError(new[] { "Password is not right" });
            var claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return CreateJwtToken(user.UserName, user.Email, user.Id.ToString(), claims);
        }
        public async Task<JwtSecurityToken> Login(string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
            if (user == null) throw new AuthenticationError(new[] { "User is not exist" });
            var claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return CreateJwtToken(user.UserName, user.Email, user.Id.ToString(), claims);
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
        public async Task ResetPassword(string token, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new AuthenticationError(new[] { "User is not found" });
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded) throw new AuthenticationError(result.Errors);
        }
        private JwtSecurityToken CreateJwtToken(string username, string email, string id, IEnumerable<Claim>? additional_claims = null)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, id)
            };
            if (additional_claims != null)
            {
                claims.AddRange(additional_claims);
            }
            var jwt = new JwtSecurityToken(
                    issuer: _configuration["AuthOptions:Issuer"],
                    audience: _configuration["AuthOptions:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["AuthOptions:Key"])),
                        SecurityAlgorithms.HmacSha256)
                    );

            return jwt;
        }
    }
}
