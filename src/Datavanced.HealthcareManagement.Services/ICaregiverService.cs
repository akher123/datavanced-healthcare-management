using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Data.Repository;
using Datavanced.HealthcareManagement.Services.DTO;
using Datavanced.HealthcareManagement.Shared.Pagination;

namespace Datavanced.HealthcareManagement.Services;

public interface ICaregiverService
{
    Task<IEnumerable<CaregiverDto>> SearchCaregiversAsync(string keyword);
    Task<PaginationResult<CaregiverDto>> GetCaregiverPaginationAsync(PaginationRequest query, CancellationToken cancellationToken);
}
public class CaregiverService : ICaregiverService
{
    private readonly ICaregiverRepository _repository;

    public CaregiverService(ICaregiverRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResult<CaregiverDto>> GetCaregiverPaginationAsync(PaginationRequest query, CancellationToken cancellationToken)
    {
        var caregivers = await _repository.GetCaregiverFilteredAsync(query, cancellationToken);

        var dtos = caregivers.Data.Select(p => new CaregiverDto
        {
            CaregiverId = p.CaregiverId,
            OfficeId = p.OfficeId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            OfficeName = p.Office.OfficeName,
            Phone = p.Phone,
            Email = p.Email,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt

        });

        return new PaginationResult<CaregiverDto>(caregivers.PageIndex, caregivers.PageSize, caregivers.TotalCount, dtos);
    }

    public async Task<IEnumerable<CaregiverDto>> SearchCaregiversAsync(string keyword)
    {
        var caregivers = await _repository.SearchCaregiversAsync(keyword);

        // Map entity to DTO
        return caregivers.Select(c => new CaregiverDto
        {
            CaregiverId = c.CaregiverId,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Phone = c.Phone
        });
    }
}