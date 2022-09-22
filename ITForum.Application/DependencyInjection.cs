using ITForum.Application.Common.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ITForum.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
            return services;
        }
    }
}
