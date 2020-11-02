using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Infrastructure.Domain.UserProfiles
{
    public class UserProfileTypeConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        private const int MaxFirebaseAuthIdLength = 128;
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.AuthUserId)
                .IsUnique();

            builder.Property(u => u.AuthUserId)
                .HasMaxLength(MaxFirebaseAuthIdLength);

            builder.Property(u => u.FirstName)
                .HasMaxLength(StringLengths.VeryShort);

            builder.Property(u => u.LastName)
                .HasMaxLength(StringLengths.VeryShort);
        }
    }
}