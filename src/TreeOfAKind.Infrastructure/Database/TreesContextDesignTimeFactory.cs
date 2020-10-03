using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TreeOfAKind.Infrastructure.SeedWork;

namespace TreeOfAKind.Infrastructure.Database
{
    public class TreesContextDesignTimeFactory : IDesignTimeDbContextFactory<TreesContext>
    {
        public TreesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TreesContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=TreeOfAKind;Trusted_Connection=True;");
            optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
            return new TreesContext(optionsBuilder.Options);
        }
    }
}