using ITForum.Api.Models.Auth;
using ITForum.Application.Common.Exceptions;
using ITForum.Domain.Errors;
using ITForum.Persistance.TempEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITForum.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ItForumUser> _userManager;
        private readonly RoleManager<ItForumRole> _roleManager;

        public AuthController(
            IConfiguration configuration,
            UserManager<ItForumUser> userManager,
            RoleManager<ItForumRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
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
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <returns>Returns TokenVm</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        /// <response code="200">Success</response>
        /// todo: 400 code(bad request)
        /// todo: 500? internal error
        /// <returns>Returns TokenVm</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
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
            
            try
            {
                if (!result.Succeeded) throw new AuthenticationError(result.Errors);
            } catch (AuthenticationError e)
            {
                throw e;
                //todo: process identityerror or create new error
            }
            //todo: add base roles creating
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new ItForumRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new ItForumRole(UserRoles.User));
            ///////////////
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
