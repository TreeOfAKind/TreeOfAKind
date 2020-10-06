using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Infrastructure.Processing.Outbox;

namespace TreeOfAKind.Infrastructure.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}