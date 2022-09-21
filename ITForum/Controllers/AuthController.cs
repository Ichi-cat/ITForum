﻿using ITForum.Api.Models.Auth;
using ITForum.Controllers;
using ITForum.Domain.Errors;
using ITForum.Persistance.TempEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
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
        [HttpPost]
        public async Task<ActionResult> SignIn([FromBody]SignInModel model)
        {
            //todo: validation
            if (!ModelState.IsValid) throw new Exception();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
                //create error
                throw new Exception("User not found");
            if(!await _userManager.CheckPasswordAsync(user, model.Password))
                //create error
                throw new Exception("Password is not right");
            var token = JwtTokenGenerator(user.UserName, user.Email, user.Id.ToString());
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        [HttpPost]
        public async Task<ActionResult> SignUp([FromBody]SignUpModel model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
                //create error
                throw new Exception("User is already exists");
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
            var token = JwtTokenGenerator(user.UserName, user.Email, user.Id.ToString());
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        [NonAction]
        private JwtSecurityToken JwtTokenGenerator(string userName, string email, string id)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, id)
            };
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
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
