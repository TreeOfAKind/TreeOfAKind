using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;

namespace TreeOfAKind.Application.Ping
{
    public class PingCommandHandler : ICommandHandler<PingCommand, Unit>
    {
        public async Task<Unit> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Unit.Value);
        }
    }
}