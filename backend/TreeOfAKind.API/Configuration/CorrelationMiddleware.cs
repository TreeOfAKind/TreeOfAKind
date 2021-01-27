using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TreeOfAKind.API.Configuration
{
    internal class CorrelationMiddleware
    {
        internal const string CorrelationHeaderKey = "CorrelationId";

        private readonly RequestDelegate _next;

        public CorrelationMiddleware(
            RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = Guid.NewGuid();

            context.Request?.Headers.Add(CorrelationHeaderKey, correlationId.ToString());

            context.Response?.OnStarting(state => {
                var httpContext = (HttpContext)state;
                httpContext.Response?.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
                return Task.CompletedTask;
            }, context);

            await this._next.Invoke(context);
        }
    }
}
