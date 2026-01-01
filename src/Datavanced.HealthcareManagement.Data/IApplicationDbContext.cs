using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Datavanced.HealthcareManagement.Data;

public interface IApplicationDbContext
{

    DbSet<Office> Offices { get; }
    DbSet<Caregiver> Caregivers { get; }
    DbSet<Patient> Patients { get; }
    DbSet<PatientCaregiver> PatientCaregivers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
