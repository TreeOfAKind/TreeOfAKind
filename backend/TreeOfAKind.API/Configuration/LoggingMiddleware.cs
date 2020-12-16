using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Application.Configuration.Validation;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.API.Configuration
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        public LoggingMiddleware(RequestDelegate next,
            ILogger logger, IExecutionContextAccessor executionContextAccessor)
        {
            _next = next;
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.Push(
                new RequestLogEnricher(_executionContextAccessor)))
            {
                context.Request.Headers.TryGetValue("Content-Type", out var contentType);

                _logger.Verbose("Processing request {@Path} with content type {@ContentType} and method {@Method}", context.Request.Path.Value,
                    contentType, context.Request.Method);
                try
                {
                    await _next(context);
                }
                catch (Exception exception)
                    when (exception is not InvalidCommandException &&
                          exception is not BusinessRuleValidationException &&
                          exception is not UnauthorizedException)
                {
                    _logger.Error(exception, "Unexpected exception");
                    throw;
                }
                finally
                {
                    _logger.Verbose("Processed request {@Path} with status code {@StatusCode}", context.Request.Path.Value,
                        context.Response.StatusCode);
                }
            }
        }

        private class RequestLogEnricher : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _executionContextAccessor;
            public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
            {
                _executionContextAccessor = executionContextAccessor;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_executionContextAccessor.IsAvailable)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}
