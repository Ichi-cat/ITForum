using ITForum.Application.Interfaces;
using ITForum.Domain.ItForumUser;
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
            var authConnectionString = configuration["AuthDbConnection"];
            services.AddDbContext<IItForumDbContext, ItForumDbContext>(options =>
            {
                options.UseSqlite(connectingString);
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<IAuthDbContext, AuthDbContext>(options =>
            {
                options.UseSqlite(authConnectionString);
                options.EnableSensitiveDataLogging();
            });

            services.AddIdentity<ItForumUser, ItForumRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
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
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            
            return services;
        }
    }
}
