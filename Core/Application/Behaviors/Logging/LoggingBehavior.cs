using Application.Logging;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Behaviors.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ILoggingRequest
    {

        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<LogParameter> logParameters = new(){new LogParameter
            {
                Type = request.GetType().Name,
                Value = request
            }};

            LogDetail logDetail = new()
            {
                Parameters = logParameters,
                MethodName = next.Method.Name,
                ResponseName = typeof(TResponse).Name,
                RequestName = typeof(TRequest).Name,
            };

            _logger.LogInformation(JsonSerializer.Serialize(logDetail));

            return await next();
        }
    }
}
