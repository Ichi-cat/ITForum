using ITForum.Application.Interfaces;
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
            services.AddDbContext<IItForumDbContext, ITForumDbContext>(options =>
            {
                options.UseSqlServer(connectingString);
            });
            
            return services;
        }
    }
}
