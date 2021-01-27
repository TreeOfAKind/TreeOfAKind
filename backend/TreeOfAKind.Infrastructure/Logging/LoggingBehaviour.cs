using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.Configuration.Validation;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Infrastructure.Processing.Outbox;

namespace TreeOfAKind.Infrastructure.Logging
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger logger)
        {
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IRecurringCommand)
            {
                return await next();
            }

            using (
                LogContext.Push(
                    new CommandLogEnricher(request as IRequest<TResponse>)))
            {
                try
                {
                    _logger.Information(
                        "Executing command {@Command}",
                        request);

                    var result = await next();

                    this._logger.Information("Command processed successful, result {@Result}", result);

                    return result;
                }
                catch (InvalidCommandException exception)
                {
                    _logger.Warning("Invalid command. Reason: {@Details}", exception.Details);
                    throw;
                }
                catch (UnauthorizedException exception)
                {
                    _logger.Warning("Unauthorized command. Reason: {@Details}", exception.Details);
                    throw;
                }
                catch (BusinessRuleValidationException exception)
                {
                    _logger.Warning("Business rule broken. Reason: {@Details}", exception.Details);
                    throw;
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Command processing failed");
                    throw;
                }
            }
        }

        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly IRequest<TResponse> _command;

            public CommandLogEnricher(IRequest<TResponse> command)
            {
                _command = command;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_command is ICommand<TResponse> command)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
                        new ScalarValue($"Command:{command.Id.ToString()}")));
                }
            }
        }
    }
}
