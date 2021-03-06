﻿using System;
using System.Linq.Expressions;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.UserProfiles
{
    public class UserProfileEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
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
                .HasMaxLength(StringLengths.Short);

            builder.Property(u => u.LastName)
                .HasMaxLength(StringLengths.Short);

            builder.Property(u => u.BirthDate)
                .HasColumnType("date");

            builder.Property(u => u.ContactEmailAddress)
                .HasConversion
                (mailAddress => mailAddress == null ? null : mailAddress.Address,
                    stringAddress => new MailAddress(stringAddress))
                .HasMaxLength(StringLengths.EmailMaxLength);
        }
    }
}
