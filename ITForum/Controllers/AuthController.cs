using ITForum.Api.Models.Auth;
using ITForum.Api.ViewModels;
using ITForum.Application.Common.Exceptions;
using ITForum.Application.Common.Exceptions.Generals;
using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITForum.Application.Interfaces;
using ITForum.Application.Services;

namespace ITForum.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ItForumUser> _userManager;
        private readonly RoleManager<ItForumRole> _roleManager;
        private readonly IFacebookAuthentication _facebookAuthentication;
        private readonly IGitHubAuthentication _gitHubAuthentication;
        

        public AuthController(
            IConfiguration configuration,
            UserManager<ItForumUser> userManager,
            RoleManager<ItForumRole> roleManager,
            IFacebookAuthentication facebookAuthentication,
            IGitHubAuthentication gitHubAuthentication)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _facebookAuthentication = facebookAuthentication;
            _gitHubAuthentication = gitHubAuthentication;
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
        public async Task<ActionResult<TokenVm>> SignIn([FromBody]SignInModel model)
        {
            //todo: validation
            if (!ModelState.IsValid) throw new Exception();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
                //create error
                throw new UserAuthException("User not found");
            if(!await _userManager.CheckPasswordAsync(user, model.Password))
                //create error
                throw new UserAuthException("Password is not right");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = JwtTokenGenerator(claims);
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
        public async Task<ActionResult<TokenVm>> SignUp([FromBody]SignUpModel model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
                //create error
                throw new UserAuthException("User is already exists");
            if (!ModelState.IsValid) throw new Exception();
            ItForumUser user = new ItForumUser{
                Email = model.Email,
                UserName = model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddLoginAsync(user, new UserLoginInfo("Faceb", "213123", "Name"));
            
            try
            {
                if (!result.Succeeded) throw new AuthenticationError(result.Errors);
            } catch (AuthenticationError e)
            {
                throw e;
                //todo: process identityerror or create new error
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = JwtTokenGenerator(claims);
            return Ok(new TokenVm
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }
        [HttpPost("facebook")]
        public async Task<ActionResult<TokenVm>> SignInFacebookAsync(string token)
        {
            //add error processing
            var fbTokenValidation = await _facebookAuthentication.ValidateToken(token);
            if (!fbTokenValidation.Data.IsValid)
            {
                //create exception if token is invalid
                throw new Exception("Token is not valid");
            }
            var userInformation = await _facebookAuthentication.GetUserInformation(token);
            var user = await _userManager.FindByLoginAsync("Facebook", userInformation.Id);
            if(user == null)
            {
                //let user choose username
                user = new ItForumUser
                {
                    Email = userInformation.Email,
                    UserName = userInformation.Name.Trim().Replace(" ", "_")
                };
                var isSucc = await _userManager.CreateAsync(user);
                if (!isSucc.Succeeded)
                {
                    //create exception if user is not created
                    throw new Exception("Something went wrong");
                }
                await _userManager.AddLoginAsync(user, new UserLoginInfo("Facebook", userInformation.Id, "Facebook"));
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //todo: need refactoring
            var jwtToken = JwtTokenGenerator(claims);
            return Ok(new TokenVm
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Expiration = jwtToken.ValidTo
            });
        }
        [HttpPost("github")]
        public async Task<ActionResult<TokenVm>> SignInGitHubAsync(string code)
        {
            //add error processing
            //code is expired
            var access_token = await _gitHubAuthentication.GetAccessToken(code);
            var userInformation = await _gitHubAuthentication.GetUserInformation(access_token.Token);
            var user = await _userManager.FindByLoginAsync("GitHub", userInformation.Id.ToString());
            if (user == null)
            {
                user = new ItForumUser
                {
                    Email = userInformation.Email ??
                        (await _gitHubAuthentication.GetUserEmails(access_token.Token)).FirstOrDefault(email => email.Primary)?.Email
                        ?? "",
                    UserName = userInformation.Login.Trim().Replace(" ", "_")
                };
                //add username duplication verification
                var succ = await _userManager.CreateAsync(user);
                if (!succ.Succeeded)
                {
                    //create exception if user is not created
                    throw new Exception("Something went wrong");
                }
                await _userManager.AddLoginAsync(user, new UserLoginInfo("GitHub", userInformation.Id.ToString(), "GitHub"));
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //todo: need refactoring
            var jwtToken = JwtTokenGenerator(claims);
            return Ok(new TokenVm
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Expiration = jwtToken.ValidTo
            });
        }
        [NonAction]
        private JwtSecurityToken JwtTokenGenerator(IEnumerable<Claim> claims)
        {
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
