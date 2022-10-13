using MediatR;
using Microsoft.Extensions.Logging;

namespace ITForum.Application.Common.Behaviors
{
    public class LoggingPipeline<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingPipeline(ILogger<Mediator> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var command = typeof(TRequest).Name;
            _logger.LogInformation("Command name: {command} Details: {@request}", command, request);
            return await next();
        }
    }
}
