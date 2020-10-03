using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.Trees
{
    public class TreeEntityTypeConfiguration : IEntityTypeConfiguration<Tree>
    {
        public void Configure(EntityTypeBuilder<Tree> builder)
        {
            builder.ToTable("Trees", SchemaNames.Trees);

            builder.HasKey(t => t.Id);

            builder.Property("_name")
                .HasColumnName("Name")
                .HasMaxLength(255);
        }
    }
}