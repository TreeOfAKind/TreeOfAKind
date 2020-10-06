using System;
using System.Threading.Tasks;

namespace TreeOfAKind.Infrastructure.Processing
{
    public interface ICommandsDispatcher
    {
        Task DispatchCommandAsync(Guid id);
    }
}
