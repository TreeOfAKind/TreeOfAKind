using MediatR;
using TreeOfAKind.Application.Configuration.Commands;

namespace TreeOfAKind.Infrastructure.Processing.Outbox
{
    public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}