using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Data.Repository;

public interface IOfficeRepository
{
    Task<IEnumerable<Office>> SearchCOfficesAsync(string searchTerm);
}
public class OfficeRepository : IOfficeRepository
{
    private readonly IApplicationDbContext dbContext;

    public OfficeRepository(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<IEnumerable<Office>> SearchCOfficesAsync(string searchTerm)
    {
        // Start with only active Offices
        var query = dbContext.Offices
            .Where(c => c.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.OfficeName, $"%{searchTerm}%") ||
                EF.Functions.Like(c.AddressLine, $"%{searchTerm}%") ||
                EF.Functions.Like(c.Phone ?? string.Empty, $"%{searchTerm}%")
            );
        }
        return await query.ToListAsync();
    }
}