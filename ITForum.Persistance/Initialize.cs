using ITForum.Api.Models.Auth;
using ITForum.Persistance.TempEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITForum.Persistance
{
    public static class Initialize
    {
        public static void Initial(ItForumDbContext context)
        {

        }
        public static async Task CreateTestUser(UserManager<ItForumUser> userManager, RoleManager<ItForumRole> roleManager)
        {
            if (userManager.FindByNameAsync(TestUser.name).GetAwaiter().GetResult() == null)
            {
                ItForumUser user = new ItForumUser
                {
                    Id = TestUser.id,
                    Email = TestUser.email,
                    UserName = TestUser.name
                };
                var result = await userManager.CreateAsync(user, TestUser.password);
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new ItForumRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new ItForumRole(UserRoles.User));
                
                if (await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }
            }
        }
        public static string CreateTestUserJwt(IConfiguration configuration)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, TestUser.email),
                new Claim(ClaimTypes.Name, TestUser.name),
                new Claim(JwtRegisteredClaimNames.Jti, TestUser.id.ToString())
            };

            var userRoles = new List<string>{ UserRoles.User };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = JwtTokenGenerator(claims, configuration);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static JwtSecurityToken JwtTokenGenerator(IEnumerable<Claim> claims, IConfiguration configuration)
        {
            var jwt = new JwtSecurityToken(
                    issuer: configuration["AuthOptions:Issuer"],
                    audience: configuration["AuthOptions:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["AuthOptions:Key"])),
                        SecurityAlgorithms.HmacSha256)
                    );

            return jwt;
        }
    }
    public static class TestUser
    {
        public static Guid id = Guid.Parse("57604FA6-E890-472E-A224-1259DBE4C950");
        public static string email = "kentMisha@gmail.com";
        public static string name = "kentMisha";
        public static string password = "PaSSword123@dsa-555";
    }
}
