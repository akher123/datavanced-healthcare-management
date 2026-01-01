using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Datavanced.HealthcareManagement.Data.Repository;

public interface ICaregiverRepository
{
    Task<IEnumerable<Caregiver>> SearchCaregiversAsync(string searchTerm);
}

public class CaregiverRepository : ICaregiverRepository
{
    private readonly IApplicationDbContext dbContext;

    public CaregiverRepository(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<IEnumerable<Caregiver>> SearchCaregiversAsync(string searchTerm)
    {
        // Start with only active caregivers
        var query = dbContext.Caregivers
            .Where(c => c.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.FirstName, $"%{searchTerm}%") ||
                EF.Functions.Like(c.LastName, $"%{searchTerm}%") ||
                EF.Functions.Like(c.Phone ?? string.Empty, $"%{searchTerm}%")
            );
        }
        return await query.ToListAsync();
    }
}