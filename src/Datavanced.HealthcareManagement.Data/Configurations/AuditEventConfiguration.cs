using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datavanced.HealthcareManagement.Data.Configurations;

public class AuditEventConfiguration : IEntityTypeConfiguration<AuditEvent>
{
    public void Configure(EntityTypeBuilder<AuditEvent> builder)
    {
        // Table name
        builder.ToTable("AuditEvents");

        // Primary key
        builder.HasKey(x => x.Id);

        // User info
        builder.Property(x => x.UserId)
               .HasMaxLength(100)
               .IsRequired(false);

        builder.Property(x => x.UserName)
               .HasMaxLength(100)
               .IsRequired(false);

        builder.Property(x => x.OfficeId)
               .HasMaxLength(100)
               .IsRequired(false);

        // Entity info
        builder.Property(x => x.Entity)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.EntityId)
               .HasMaxLength(100)
               .IsRequired(false);

        // Action info
        builder.Property(x => x.Action)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.ActionType)
               .HasMaxLength(50)
               .IsRequired();

        // Optional metadata/details
        builder.Property(x => x.Details)
               .HasColumnType("nvarchar(max)")
               .IsRequired(false);

        // Timestamp
        builder.Property(x => x.Timestamp)
               .HasColumnType("datetime2")
               .IsRequired();

        // Indexes for performance
        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.OfficeId);
        builder.HasIndex(x => x.Entity);
        builder.HasIndex(x => x.ActionType);
    }
}
