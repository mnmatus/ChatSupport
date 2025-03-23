using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace ChatSupport.Application.Common.Behaviours
{
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<RequestPerformanceBehavior<TRequest, TResponse>> _logger;

        public RequestPerformanceBehavior(ILogger<RequestPerformanceBehavior<TRequest, TResponse>> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            //if request took more than 3 seconds, then log it for investigation
            if (_timer.ElapsedMilliseconds > 3000)
            {
                var requestName = $"{typeof(TRequest).Name} | {_timer.ElapsedMilliseconds}ms";
                var requestJson = JsonSerializer.Serialize(request);
                var message = $"{requestJson}";
                _logger.LogWarning($"Long Running Request: {requestName} | Request: {message} | Duration {_timer.ElapsedMilliseconds}ms.");
            }

            return response;
        }
    }
}
