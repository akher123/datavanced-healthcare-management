using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datavanced.HealthcareManagement.Data.Configurations;

public class PatientCaregiverConfiguration : IEntityTypeConfiguration<PatientCaregiver>
{
    public void Configure(EntityTypeBuilder<PatientCaregiver> builder)
    {
        // Map entity to singular table name
        builder.ToTable("PatientCaregiver");

        // Composite Primary Key
        builder.HasKey(pc => new { pc.PatientId, pc.CaregiverId });

        // Properties
        builder.Property(pc => pc.AssignedAt)
               .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(pc => pc.Patient)
               .WithMany(p => p.PatientCaregivers)
               .HasForeignKey(pc => pc.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pc => pc.Caregiver)
               .WithMany(c => c.PatientCaregivers)
               .HasForeignKey(pc => pc.CaregiverId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
