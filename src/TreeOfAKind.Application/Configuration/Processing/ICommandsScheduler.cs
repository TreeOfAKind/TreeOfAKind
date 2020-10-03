using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;

namespace TreeOfAKind.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}