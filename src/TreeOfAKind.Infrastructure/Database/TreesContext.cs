using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Infrastructure.Processing.InternalCommands;
using TreeOfAKind.Infrastructure.Processing.Outbox;

namespace TreeOfAKind.Infrastructure.Database
{
    public class TreesContext : DbContext
    {
        public DbSet<Tree> Trees { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DbSet<InternalCommand> InternalCommands { get; set; }

        public TreesContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TreesContext).Assembly);
        }
    }
}
