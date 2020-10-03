using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Infrastructure.Database;
using TreeOfAKind.Infrastructure.Processing;

namespace TreeOfAKind.Infrastructure.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TreesContext _treesContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(
            TreesContext treesContext, 
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this._treesContext = treesContext;
            this._domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this._domainEventsDispatcher.DispatchEventsAsync();
            return await this._treesContext.SaveChangesAsync(cancellationToken);
        }
    }
}