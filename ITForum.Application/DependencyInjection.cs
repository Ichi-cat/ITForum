using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using ITForum.Application.Common.Behaviors;
using ITForum.Application.Interfaces;
using ITForum.Application.Services;
using ITForum.Application.Services.IdentityService;

namespace ITForum.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
            services.AddTransient<IFacebookAuthentication, FacebookAuthentication>();
            services.AddTransient<IGitHubAuthentication, GitHubAuthentication>();
            services.AddTransient<IIdentityService, IdentityService>();
            return services;
        }
    }
}
