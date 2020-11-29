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
        internal const string treeOwners = "_treeOwners";
        internal const string ownedTrees = "_ownedTrees";
        internal const string people = "_people";
        public void Configure(EntityTypeBuilder<Tree> builder)
        {
            builder.ToTable("Trees", SchemaNames.Trees);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(StringLengths.VeryShort);

            builder.HasMany(treeOwners)
                .WithMany(ownedTrees);

            builder.OwnsMany<Person>(people, b =>
            {
                b.ToTable("People", SchemaNames.Trees);

                b.HasKey(p => p.Id);

                b.WithOwner(p => p.Tree);
            });

            builder.Ignore(t => t.TreeOwners);
            builder.Ignore(t => t.People);

        }
    }
}