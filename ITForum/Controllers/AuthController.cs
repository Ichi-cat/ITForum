using ITForum.Api.Models;
using ITForum.Api.Models.Auth;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Common.ViewModels;
using ITForum.Application.Interfaces;
using ITForum.Application.Services.IdentityService;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;

namespace ITForum.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : BaseController
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IFacebookAuthentication _facebookAuthentication;
        private readonly IGitHubAuthentication _gitHubAuthentication;
        private readonly IIdentityService _identityService;

        public AuthController(
            UserManager<ItForumUser> userManager,
            IFacebookAuthentication facebookAuthentication,
            IGitHubAuthentication gitHubAuthentication,
            IIdentityService identityService)
        {
            _userManager = userManager;
            _facebookAuthentication = facebookAuthentication;
            _gitHubAuthentication = gitHubAuthentication;
            _identityService = identityService;
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<TokenVm>> SignIn([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            var token = await _identityService.Login(new BaseUserInfoModel { UserName = model.UserName, Email = model.UserName }, model.Password);
            return Ok(token);
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<TokenVm>> SignUp([FromBody] SignUpModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));

            var token = await _identityService.CreateUser(
                new() { UserName = model.UserName, Email = model.Email }, model.Password);
            return Ok(token);
        }
        [HttpPost]
        public async Task<ActionResult<TokenVm>> RefreshToken([FromBody]RefreshTokenModel model)
        {
            var token = await _identityService.RefreshToken(model.RefreshToken, model.AccessToken);
            return Ok(token);
        }
        [AllowAnonymous]
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
                var tokenVm = await _identityService.CreateUserWithProvider(
                    new("Facebook", userInformation.Id, "Facebook"),
                    baseUserInfo);
                return Ok(tokenVm);
            }
            else
            {
                var tokenVm = await _identityService.Login("Facebook", userInformation.Id);
                return Ok(tokenVm);
            }
        }
        [AllowAnonymous]
        [HttpPost("github")]
        public async Task<ActionResult<TokenVm>> SignInGitHubAsync(string code, [FromBody] SignInWithProviderModel model)
        {
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
                var tokenVm = await _identityService.CreateUserWithProvider(
                    new("GitHub", userInformation.Id.ToString(), "GitHub"),
                    baseUserInfo);
                return Ok(tokenVm);
            }
            else
            {
                var tokenVm = await _identityService.Login("GitHub", userInformation.Id.ToString());
                return Ok(tokenVm);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody]GetTokenModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            await _identityService.SendToken(model.Email, model.RedirectUri);

            return NoContent();
        }
        [HttpPut]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            await _identityService.ResetPassword(model.Token, model.Email, model.Password);
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid) throw new AuthenticationError(ModelState.Values.SelectMany(v => v.Errors));
            await _identityService.ChangePassword(UserId, model.OldPassword, model.NewPassword);
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailModel model)
        {
            await _identityService.ChangeEmail(model.Email, UserId);
            return NoContent();
        }
    }
}
