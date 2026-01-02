using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Datavanced.HealthcareManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

    public DbSet<Office> Offices { get; set; }
    public DbSet<Caregiver> Caregivers { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientCaregiver> PatientCaregivers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //InitialSeedData.Seed(builder);

        base.OnModelCreating(builder);
    }
}
