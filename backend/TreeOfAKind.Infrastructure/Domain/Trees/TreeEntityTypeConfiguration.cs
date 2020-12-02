using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.Trees
{
    public class TreeEntityTypeConfiguration : IEntityTypeConfiguration<Tree>
    {
        public void Configure(EntityTypeBuilder<Tree> builder)
        {
            builder.ToTable("Trees", SchemaNames.Trees);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(StringLengths.VeryShort);

            builder.OwnsMany(t => t.TreeOwners, b =>
            {
                b.ToTable("TreeUserProfile", SchemaNames.Trees);
                b.HasKey(to => new {to.TreeId, to.UserId});
            });

            builder.OwnsMany<Person>(t => t.People, b =>
            {
                b.ToTable("People", SchemaNames.Trees);

                b.HasKey(p => p.Id);

                b.WithOwner(p => p.Tree);
            });
        }
    }
}