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

            builder.HasMany<UserProfile>(treeOwners)
                .WithMany(ownedTrees);

            builder.HasMany<Person>(people)
                .WithOne(p => p.Tree);

        }
    }
}