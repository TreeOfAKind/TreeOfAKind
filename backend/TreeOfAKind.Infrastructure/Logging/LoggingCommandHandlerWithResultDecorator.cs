using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Infrastructure.Processing.Outbox;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace TreeOfAKind.Infrastructure.Logging
{
    internal class LoggingCommandHandlerWithResultDecorator<T, TResult> : IRequestHandler<T, TResult> where T : IRequest<TResult>
    {
        private readonly ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IRequestHandler<T, TResult> _decorated;

        public LoggingCommandHandlerWithResultDecorator(
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            IRequestHandler<T, TResult> decorated)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
            _decorated = decorated;
        }
        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            if (command is IRecurringCommand)
            {
                return await _decorated.Handle(command, cancellationToken);
            }

            using (
                LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher(command)))
            {
                try
                {
                    this._logger.Information(
                        "Executing command {@Command}",
                        command);

                    var result = await _decorated.Handle(command, cancellationToken);

                    this._logger.Information("Command processed successful, result {Result}", result);

                    return result;
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception, "Command processing failed");
                    throw;
                }
            }
        }

        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly IRequest<TResult> _command;

            public CommandLogEnricher(IRequest<TResult> command)
            {
                _command = command;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_command is ICommand<TResult> command)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
                        new ScalarValue($"Command:{command.Id.ToString()}")));
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