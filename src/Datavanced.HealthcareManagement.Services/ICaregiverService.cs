using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Data.Repository;
using Datavanced.HealthcareManagement.Services.DTO;

namespace Datavanced.HealthcareManagement.Services;

public interface ICaregiverService
{
    Task<IEnumerable<CaregiverDto>> SearchCaregiversAsync(string keyword);
}
public class CaregiverService : ICaregiverService
{
    private readonly ICaregiverRepository _repository;

    public CaregiverService(ICaregiverRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CaregiverDto>> SearchCaregiversAsync(string keyword)
    {
        var caregivers = await _repository.SearchCaregiversAsync(keyword);

        // Map entity to DTO
        return caregivers.Select(c => new CaregiverDto
        {
            CaregiverId = c.CaregiverId,
            FullName = $"{c.FirstName} {c.LastName}",
            Phone = c.Phone
        });
    }
}