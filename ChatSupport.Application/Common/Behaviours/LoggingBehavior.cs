using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ChatSupport.Application.Common.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();
            var requestJson = JsonSerializer.Serialize(request);
            var responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation($"[LOGGING BEHAVIOR] Request {typeof(TResponse).Name}. Request Payload: {requestJson} | Response: {typeof(TResponse).Name} Response Payload: {responseJson}");
            return response;
        }
    }
}
