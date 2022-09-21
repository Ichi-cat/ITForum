using ITForum.Application.Interfaces;
using ITForum.Persistance.TempEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITForum.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectingString = configuration["DbConnection"];
            services.AddDbContext<IItForumDbContext, ItForumDbContext>(options =>
            {
                options.UseSqlServer(connectingString);
            });

            services.AddIdentity<ItForumUser, ItForumRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 6,
                    RequiredUniqueChars = 1,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
            })
                .AddEntityFrameworkStores<ItForumDbContext>()
                .AddDefaultTokenProviders();
            
            return services;
        }
    }
}
