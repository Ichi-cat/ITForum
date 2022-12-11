using ITForum.Domain.ItForumUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITForum.Persistance
{
    public static class Initialize
    {
        public static async Task Initial(ItForumDbContext context, UserManager<ItForumUser> userManager, RoleManager<ItForumRole> roleManager)
        {
            //ask
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new ItForumRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new ItForumRole(UserRoles.User));
        }
        public static async Task CreateTestUser(UserManager<ItForumUser> userManager, RoleManager<ItForumRole> roleManager)
        {
            if (await userManager.FindByNameAsync(TestUser.name) == null)
            {
                ItForumUser user = new ItForumUser
                {
                    Id = TestUser.id,
                    Email = TestUser.email,
                    UserName = TestUser.name,
                    FirstName = TestUser.firstName,
                    LastName = TestUser.lastName,
                    Description = TestUser.description,
                    Avatar = TestUser.avatar,
                    Location = TestUser.location,
                    BirthLocation = TestUser.birthLocation,
                    BirthDate = TestUser.birthDate,
                    Study = TestUser.study,
                    Work = TestUser.work,
                    TimeZone = TestUser.timeZone
                };
                var result = await userManager.CreateAsync(user, TestUser.password);
                
                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
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

            var userRoles = new List<string> { UserRoles.Admin };
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
        public static Guid id = Guid.Parse("65AA4F89-DEBE-4A65-AEC0-AA67D6A51612");
        public static string email = "mihail@gmail.com";
        public static string name = "mihail";
        public static string password = "PaSSword123@dsa-555";
        public static string description = "I am a test user and i don't have a soul. But i can think, so i am not a robot. I guess you're cute";
        public static string avatar = "https://avatars.githubusercontent.com/u/75914175?v=4";
        public static string firstName = "Miha";
        public static string lastName = "Olifer";   
        public static string location = "Zaporizhya/Ukraine";
        public static string birthLocation = "Zaporizhya/Ukraine";
        public static DateTime birthDate = new DateTime(1889, 4, 20);
        public static string study = "KhPI";
        public static string work = "McDonald's";
        public static string timeZone = "Europe/Ukraine";
    }
}
