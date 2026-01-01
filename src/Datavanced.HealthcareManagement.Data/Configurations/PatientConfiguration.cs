using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datavanced.HealthcareManagement.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        // Map entity to singular table name
        builder.ToTable("Patient");

        // Primary Key
        builder.HasKey(p => p.PatientId);

        // Properties
        builder.Property(p => p.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Phone)
               .HasMaxLength(20);

        builder.Property(p => p.Email)
               .HasMaxLength(150);

        builder.Property(p => p.DateOfBirth)
               .HasColumnType("date");

        builder.Property(p => p.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.IsActive)
               .HasDefaultValue(true);

        // Relationships
        builder.HasOne(p => p.Office)
               .WithMany(o => o.Patients)
               .HasForeignKey(p => p.OfficeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.PatientCaregivers)
               .WithOne(pc => pc.Patient)
               .HasForeignKey(pc => pc.PatientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
