using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.UserProfiles
{
    public class UserProfileTypeConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles", SchemaNames.Trees);

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.UserAuthId)
                .IsUnique();

            builder.Property(u => u.UserAuthId)
                .HasMaxLength(StringLengths.AuthIdLength);

            builder.Property(u => u.FirstName)
                .HasMaxLength(StringLengths.VeryShort);

            builder.Property(u => u.LastName)
                .HasMaxLength(StringLengths.VeryShort);

            builder.Property(u => u.BirthDate)
                .HasColumnType("date");

            builder.Ignore(u => u.OwnedTrees);
        }
    }
}
