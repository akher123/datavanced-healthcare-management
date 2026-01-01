using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datavanced.HealthcareManagement.Data.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        // Map entity to singular table name
        builder.ToTable("Office");

        // Primary Key
        builder.HasKey(o => o.OfficeId);

        // Properties
        builder.Property(o => o.OfficeName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(o => o.AddressLine)
               .HasMaxLength(250);

        builder.Property(o => o.Phone)
               .HasMaxLength(20);

        builder.Property(o => o.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(o => o.IsActive)
               .HasDefaultValue(true);

        // Relationships
        builder.HasMany(o => o.Caregivers)
               .WithOne(c => c.Office)
               .HasForeignKey(c => c.OfficeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Patients)
               .WithOne(p => p.Office)
               .HasForeignKey(p => p.OfficeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
