using ITForum.Api.Models;
using ITForum.Api.Models.Auth;
using ITForum.Api.ViewModels;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Interfaces;
using ITForum.Application.Services.IdentityService;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NETCore.MailKit.Core;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace ITForum.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IFacebookAuthentication _facebookAuthentication;
        private readonly IGitHubAuthentication _gitHubAuthentication;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ItForumUser> userManager,
            IFacebookAuthentication facebookAuthentication,
            IGitHubAuthentication gitHubAuthentication,
            IIdentityService identityService,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _facebookAuthentication = facebookAuthentication;
            _gitHubAuthentication = gitHubAuthentication;
            _identityService = identityService;
            _emailService = emailService;
            _configuration = configuration;
        }
        /// <summary>
        /// Login action
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "userName": "myUserName",
        ///         "password": "somePassword"
        ///     }
        ///     
        /// </remarks>
        /// <param name="model">SignInModel</param>
        /// <returns>Returns TokenVm</returns>
        [SwaggerResponse(200)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [HttpPost]
        public async Task<ActionResult<TokenVm>> SignIn([FromBody] SignInModel model)
        {
            //todo: validation
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            var token = await _identityService.Login(new BaseUserInfoModel { UserName = model.UserName, Email = model.UserName }, model.Password);
            return Ok(new TokenVm
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }
        /// <summary>
        /// Registration action
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "userName": "myUserName",
        ///         "password": "myPassword",
        ///         "confirmPassword": "myPassword",
        ///         "email": "user@example.com"
        ///     }
        ///     
        /// </remarks>
        /// <param name="model">SignUpModel</param>
        /// <returns>Returns TokenVm</returns>
        [SwaggerResponse(200)]
        [SwaggerResponse(400, type: typeof(GeneralExceptionVm))]
        [HttpPost]
        public async Task<ActionResult<TokenVm>> SignUp([FromBody] SignUpModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));

            var token = await _identityService.CreateUser(
                new() { UserName = model.UserName, Email = model.Email }, model.Password);
            return Ok(new TokenVm
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }
        [HttpPost("facebook")]
        public async Task<ActionResult<TokenVm>> SignInFacebookAsync(string token, [FromBody] SignInWithProviderModel model)
        {
            var fbTokenValidation = await _facebookAuthentication.ValidateToken(token);
            if (!fbTokenValidation.Data.IsValid)
            {
                throw new AuthenticationError(new[] { "Token is not valid" });
            }
            var userInformation = await _facebookAuthentication.GetUserInformation(token);
            var user = await _userManager.FindByLoginAsync("Facebook", userInformation.Id);
            if (user == null)
            {
                //let user choose username
                var baseUserInfo = new BaseUserInfoModel()
                {
                    Email = userInformation.Email,
                    UserName = userInformation.Name.Trim().Replace(" ", "_"),
                    IsEmailConfirmed = true
                };
                if (model.Email != null)
                {
                    baseUserInfo.Email = model.Email;
                    baseUserInfo.IsEmailConfirmed = false;
                }
                if (model.UserName != null)
                {
                    baseUserInfo.UserName = model.UserName;
                }
                JwtSecurityToken jwtToken = await _identityService.CreateUserWithProvider(
                    new("Facebook", userInformation.Id, "Facebook"),
                    baseUserInfo);
                return Ok(new TokenVm
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo
                });
            }
            else
            {
                //facebook const
                JwtSecurityToken jwtToken = await _identityService.Login("Facebook", userInformation.Id);
                return Ok(new TokenVm
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo
                });
            }
        }

        [HttpPost("github")]
        public async Task<ActionResult<TokenVm>> SignInGitHubAsync(string code, [FromBody] SignInWithProviderModel model)
        {
            //add error processing
            //code is expired
            var access_token = await _gitHubAuthentication.GetAccessToken(code);
            var userInformation = await _gitHubAuthentication.GetUserInformation(access_token.Token);
            var user = await _userManager.FindByLoginAsync("GitHub", userInformation.Id.ToString());
            if (user == null)
            {
                var baseUserInfo = new BaseUserInfoModel()
                {
                    Email = userInformation.Email ??
                        (await _gitHubAuthentication.GetUserEmails(access_token.Token)).FirstOrDefault(email => email.Primary)?.Email
                        ?? "",
                    UserName = userInformation.Login.Trim().Replace(" ", "_"),
                    IsEmailConfirmed = true
                };
                if (model.Email != null)
                {
                    baseUserInfo.Email = model.Email;
                    baseUserInfo.IsEmailConfirmed = false;
                }
                if (model.UserName != null)
                {
                    baseUserInfo.UserName = model.UserName;
                }
                JwtSecurityToken jwtToken = await _identityService.CreateUserWithProvider(
                    new("GitHub", userInformation.Id.ToString(), "GitHub"),
                    baseUserInfo);
                //add username duplication verification
                return Ok(new TokenVm
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo
                });
            }
            else
            {
                JwtSecurityToken jwtToken = await _identityService.Login("GitHub", userInformation.Id.ToString());
                return Ok(new TokenVm
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo
                });
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody]GetTokenModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User exists");
            if (!user.EmailConfirmed) throw new Exception("Email is not confirmed. You can't reset password");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            IDictionary<string, string?> query = new Dictionary<string, string?>() { { "token", token }, { "email", user.Email } };
            var redirect_uri = new Uri(QueryHelpers.AddQueryString(model.RedirectUri.AbsoluteUri, query));

            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Html/ResetPassword.html"));

            string data = System.IO.File.ReadAllText(path).Replace("linkedUrl", HtmlEncoder.Default.Encode(redirect_uri.ToString()));
            await _emailService.SendAsync("staske11111@gmail.com", "Reset password", data, isHtml: true);

            return NoContent();
        }
        [HttpPut]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User is not found");
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!result.Succeeded) throw new Exception("Token is failed");
            return NoContent();
        }
    }
}
