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
            services.AddDbContext<ITForumDbContext>(options =>
            {
                options.UseSqlServer(connectingString);
            });
            services.AddScoped<ITForumDbContext>(provider =>
                provider.GetService<ITForumDbContext>());//TODO:Change class to interface
            return services;
        }
    }
}
