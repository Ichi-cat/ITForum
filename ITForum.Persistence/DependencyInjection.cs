using ITForum.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ITForum.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<IItForumDbContext, ItForumDbContext>(options =>
            {
                options.UseInMemoryDatabase("4C2F7D17-5D20-49D8-A133-DBC0A58B0ABA");
            });
            return services;
        }
    }
}
