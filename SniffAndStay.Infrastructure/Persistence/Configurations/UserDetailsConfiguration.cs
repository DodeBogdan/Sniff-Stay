using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SniffAndStay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Infrastructure.Persistence.Configurations
{
    public class UserDetailsConfiguration : IEntityTypeConfiguration<UserDetails>
    {
        public void Configure(EntityTypeBuilder<UserDetails> builder)
        {
            builder.ToTable("UserDetails");

            builder.HasKey(ud => ud.UserId);

            builder.Property(user => user.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(user => user.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(user => user.DateOfBirth)
                .IsRequired();

            builder.Property(user => user.ProfilePicture)
                .HasMaxLength(256);

            builder.Property(user => user.Gender)
                .HasDefaultValue(Gender.Unknown);

            builder.HasOne(ud => ud.User)
            .WithOne(user => user.UserDetails)
            .HasForeignKey<UserDetails>(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
