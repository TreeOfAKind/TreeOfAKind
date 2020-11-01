using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Processing.InternalCommands;
using TreeOfAKind.Infrastructure.Processing.Outbox;

namespace TreeOfAKind.Infrastructure.Database
{
    public class TreesContext : DbContext
    {
        public DbSet<Tree> Trees => Set<Tree>();
        public DbSet<UserProfile> Users => Set<UserProfile>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        public DbSet<InternalCommand> InternalCommands  => Set<InternalCommand>();

        public TreesContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TreesContext).Assembly);
        }
    }
}