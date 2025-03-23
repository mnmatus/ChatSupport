using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace ChatSupport.Application.Common.Behaviours
{
    public class RequestExceptionActionBehavior<TRequest, TException> : IRequestExceptionAction<TRequest, TException> where TException : Exception
    {
        private readonly ILogger<RequestExceptionActionBehavior<TRequest, TException>> _logger;

        public RequestExceptionActionBehavior(ILogger<RequestExceptionActionBehavior<TRequest, TException>> logger)
        {
            _logger = logger;
        }

        public async Task Execute(TRequest request, TException exception, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(exception, $"Error Message: {exception.Message} {exception.StackTrace} | Request Name: {requestName}", request);
        }
    }
}
