using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .HasMaxLength(100);

            builder.Property(u => u.OfficeId)
                   .IsRequired();

            // Relationships
            builder.HasOne(u => u.Office)
                   .WithMany(o => o.Users)
                   .HasForeignKey(u => u.OfficeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
