using Microsoft.EntityFrameworkCore;
namespace Datavanced.HealthcareManagement.Services;

public interface ICaregiverService
{
    Task<IEnumerable<CaregiverSearchDto>> SearchCaregiversAsync(string keyword,int officeId);
    Task<PaginationResult<CaregiverDto>> GetCaregiverPaginationAsync(PaginationRequest query, CancellationToken cancellationToken);
    Task<ResponseMessage<CaregiverDto>> CreateAsync(CreateCaregiverDto dto, CancellationToken cancellationToken);
    Task<ResponseMessage<bool>> UpdateCaregiverAsync(int id, UpdateCaregiverDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteCaregiverByIdAsync(int id, CancellationToken cancellationToken);
}

public class CaregiverService : ICaregiverService
{
    private readonly ICaregiverRepository _repository;

    public CaregiverService(ICaregiverRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseMessage<CaregiverDto>> CreateAsync(CreateCaregiverDto dto, CancellationToken cancellationToken)
    {
        var caregiver = new Caregiver
        {
            OfficeId = dto.OfficeId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
       await _repository.CreateAsync(caregiver,cancellationToken);

       return new ResponseMessage<CaregiverDto>
        {

            Result = new CaregiverDto
            {
                CaregiverId = caregiver.CaregiverId,
                OfficeId = caregiver.OfficeId,
                FirstName = caregiver.FirstName,
                LastName = caregiver.LastName,
                Phone = caregiver.Phone,
                Email = caregiver.Email,
                IsActive = caregiver.IsActive,
                CreatedAt = caregiver.CreatedAt
            },
            Message = "Caregiver created successfully",
        };
      }

    public async Task<bool> DeleteCaregiverByIdAsync(int id, CancellationToken cancellationToken)
    {
        var caregiver = await _repository.GetCaregiverByIdAsync(id, cancellationToken);
        if (caregiver == null)
        {
            throw new NotFoundException("Caregiver not found.");
        }
        return await _repository.DeleteByIdAsync(caregiver, cancellationToken);
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

    public async Task<IEnumerable<CaregiverSearchDto>> SearchCaregiversAsync(string keyword,int officeId)
    {
        var caregivers = await _repository.SearchCaregiversAsync(keyword, officeId);

        // Map entity to DTO
        return caregivers.Select(c => new CaregiverSearchDto
        {
            CaregiverId = c.CaregiverId,
            CaregiverName = c.FirstName+" "+ c.LastName
        });
    }

    public async Task<ResponseMessage<bool>> UpdateCaregiverAsync(int id, UpdateCaregiverDto dto, CancellationToken cancellationToken)
    {
        var caregiver = new Caregiver
        {
            OfficeId = dto.OfficeId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.UpdateAsync(caregiver, cancellationToken);
        return new ResponseMessage<bool>
        {
            Result = true,
            Message = "Caregiver updated successfully.",
        };
    }
}