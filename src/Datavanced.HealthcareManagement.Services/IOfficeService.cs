using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Data.Repository;
using Datavanced.HealthcareManagement.Services.DTO;

namespace Datavanced.HealthcareManagement.Services;

public interface IOfficeService
{
    Task<IEnumerable<OfficeDto>> SearchOfficesAsync(string keyword);
}
public class OfficeService(IOfficeRepository repository) : IOfficeService
{
    private readonly IOfficeRepository _repository = repository;

    public async Task<IEnumerable<OfficeDto>> SearchOfficesAsync(string keyword)
    {
        var caregivers = await _repository.SearchCOfficesAsync(keyword);

        // Map entity to DTO
        return caregivers.Select(c => new OfficeDto
        {
            OfficeId = c.OfficeId,
            OfficeName =c.OfficeName,
            Phone = c.Phone
        });
    }
}