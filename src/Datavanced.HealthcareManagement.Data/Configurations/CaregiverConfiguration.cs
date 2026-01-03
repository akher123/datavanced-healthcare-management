using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datavanced.HealthcareManagement.Data.Configurations;

public class CaregiverConfiguration : IEntityTypeConfiguration<Caregiver>
{
    public void Configure(EntityTypeBuilder<Caregiver> builder)
    {
        // Map entity to singular table name
        builder.ToTable("Caregiver");
        // Primary Key
        builder.HasKey(c => c.CaregiverId);

        // Properties
        builder.Property(c => c.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Phone)
               .HasMaxLength(20);

        builder.Property(c => c.Email)
               .HasMaxLength(150);

        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(c => c.IsActive)
               .HasDefaultValue(true);

        // Relationships
        builder.HasOne(c => c.Office)
               .WithMany(o => o.Caregivers)
               .HasForeignKey(c => c.OfficeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.PatientCaregivers)
               .WithOne(pc => pc.Caregiver)
               .HasForeignKey(pc => pc.CaregiverId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
