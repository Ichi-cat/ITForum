using ITForum.Api.Models.Auth;
using ITForum.Api.ViewModels;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Application.Interfaces;
using ITForum.Application.Services.IdentityService;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;

namespace ITForum.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ItForumUser> _userManager;
        private readonly IFacebookAuthentication _facebookAuthentication;
        private readonly IIdentityService _identityService;

        public AuthController(
            UserManager<ItForumUser> userManager,
            IFacebookAuthentication facebookAuthentication,
            IIdentityService identityService)
        {
            _userManager = userManager;
            _facebookAuthentication = facebookAuthentication;
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
        public async Task<IActionResult> SignInFacebookAsync(string token)
        {
            var fbTokenValidation = await _facebookAuthentication.ValidateToken(token);
            if (!fbTokenValidation.Data.IsValid)
            {
                //create exception if token is invalid
                throw new Exception("Token is not valid");
            }
            var userInformation = await _facebookAuthentication.GetUserInformation(token);
            var user = await _userManager.FindByLoginAsync("Facebook", userInformation.Id);
            if (user == null)
            {
                //let user choose username
                var baseUserInfo = new BaseUserInfoModel()
                {
                    Email = userInformation.Email,
                    UserName = userInformation.Name.Trim().Replace(" ", "_")
                };
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
                JwtSecurityToken jwtToken = await _identityService.Login("Facebook", userInformation.Id.ToString());
                return Ok(new TokenVm
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo
                });
            }
        }
    }
}
