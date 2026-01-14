using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Shared.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Datavanced.HealthcareManagement.Data.Repository;

public interface ICaregiverRepository
{
    Task<IEnumerable<Caregiver>> SearchCaregiversAsync(string searchTerm, int officeId);
    Task<PaginationResult<Caregiver>> GetCaregiverFilteredAsync(PaginationRequest query, CancellationToken cancellationToken);
    Task<int> CreateAsync(Caregiver caregiver,CancellationToken cancellationToken);
    Task<int> UpdateAsync(Caregiver caregiver, CancellationToken cancellationToken);
    Task<bool> DeleteByIdAsync(Caregiver caregiver, CancellationToken cancellationToken);
    Task<Caregiver> GetCaregiverByIdAsync(int id, CancellationToken cancellationToken);
}

public class CaregiverRepository : ICaregiverRepository
{
    private readonly IApplicationDbContext dbContext;

    public CaregiverRepository(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> CreateAsync(Caregiver caregiver, CancellationToken cancellationToken)
    {
        dbContext.Caregivers.Add(caregiver);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task<bool> DeleteByIdAsync(Caregiver caregiver, CancellationToken cancellationToken)
    {
        dbContext.Caregivers.Remove(caregiver);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Caregiver> GetCaregiverByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Caregivers
               .FirstOrDefaultAsync(cg => cg.CaregiverId == id, cancellationToken);
    }

    public async Task<PaginationResult<Caregiver>> GetCaregiverFilteredAsync(
       PaginationRequest query,
       CancellationToken cancellationToken)
    {
        int pageIndex = query.PageIndex;
        int pageSize = query.PageSize;

        var caregiversQuery = dbContext.Caregivers
            .Include(c => c.Office)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = $"%{query.Keyword.Trim()}%";

            caregiversQuery = caregiversQuery.Where(c =>
                EF.Functions.Like(c.FirstName, keyword) ||
                EF.Functions.Like(c.LastName, keyword) ||
                EF.Functions.Like(c.Email, keyword) ||
                EF.Functions.Like(c.Phone, keyword) ||
                EF.Functions.Like(c.Office.OfficeName, keyword)
            );
        }

        switch (query.SortBy?.ToLower())
        {
            case "firstname":
                caregiversQuery = query.Descending
                    ? caregiversQuery.OrderByDescending(c => c.FirstName)
                    : caregiversQuery.OrderBy(c => c.FirstName);
                break;

            case "lastname":
                caregiversQuery = query.Descending
                    ? caregiversQuery.OrderByDescending(c => c.LastName)
                    : caregiversQuery.OrderBy(c => c.LastName);
                break;

            case "createdat":
                caregiversQuery = query.Descending
                    ? caregiversQuery.OrderByDescending(c => c.CreatedAt)
                    : caregiversQuery.OrderBy(c => c.CreatedAt);
                break;

            case "office":
                caregiversQuery = query.Descending
                    ? caregiversQuery.OrderByDescending(c => c.Office.OfficeName)
                    : caregiversQuery.OrderBy(c => c.Office.OfficeName);
                break;

            case "isactive":
                caregiversQuery = query.Descending
                    ? caregiversQuery.OrderByDescending(c => c.IsActive)
                    : caregiversQuery.OrderBy(c => c.IsActive);
                break;

            default:
                caregiversQuery = query.Descending
                    ? caregiversQuery.OrderByDescending(c => c.CaregiverId)
                    : caregiversQuery.OrderBy(c => c.CaregiverId);
                break;
        }

        var total = await caregiversQuery.CountAsync(cancellationToken);

        var caregivers = await caregiversQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginationResult<Caregiver>(pageIndex, pageSize, total, caregivers);
    }

    public async Task<IEnumerable<Caregiver>> SearchCaregiversAsync(string searchTerm, int officeId)
    {

        var query = dbContext.Caregivers
            .Where(c => c.IsActive && (officeId ==-1 || c.OfficeId==officeId)) // -1 for all if admin user todo logic infuture
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

    public async Task<int> UpdateAsync(Caregiver caregiver, CancellationToken cancellationToken)
    {
        dbContext.Caregivers.Update(caregiver);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}