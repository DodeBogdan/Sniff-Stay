using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SniffAndStay.Domain.Entities;
namespace SniffAndStay.Infrastructure.Persistence.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.Picture)
                .HasMaxLength(256);

            builder.Property(p => p.PetType)
                .HasDefaultValue(PetType.Unknown);

            builder.HasOne(p => p.User)
                .WithMany(user => user.Pets)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
