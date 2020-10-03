using System.Threading.Tasks;

namespace TreeOfAKind.Infrastructure.Processing
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}