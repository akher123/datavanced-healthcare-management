using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Data.Repository;
using Datavanced.HealthcareManagement.Services.DTO;
using Datavanced.HealthcareManagement.Shared;
using Datavanced.HealthcareManagement.Shared.ExceptionHelper;
using Datavanced.HealthcareManagement.Shared.Pagination;

namespace Datavanced.HealthcareManagement.Services;

public interface IPatientService
{
    Task<ResponseMessage<PaginationResult<PatientDto>>> GetFilteredAsync(PaginationRequest query, CancellationToken cancellationToken);
    Task<PatientDto?> GetPatientByIdAsync(int id, CancellationToken cancellationToken);
    Task<ResponseMessage<bool>> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken);
    Task<ResponseMessage<bool>> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResponseMessage<PaginationResult<PatientDto>>> GetFilteredAsync(PaginationRequest query, CancellationToken cancellationToken)
    {
        var patients = await _repository.GetFilteredAsync(query, cancellationToken);

        var dtos = patients.Data.Select(p => new PatientDto
        {
            PatientId = p.PatientId,
            OfficeId = p.OfficeId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Phone = p.Phone,
            Email = p.Email,
            OfficeName = p.Office?.OfficeName ?? string.Empty,
            IsActive = p.IsActive
        });


       return new ResponseMessage<PaginationResult<PatientDto>>{
            Result = new PaginationResult<PatientDto>(
                patients.PageIndex,
                patients.PageSize,
                patients.TotalCount,
                dtos
            )
        };

    }

    public async Task<PatientDto?> GetPatientByIdAsync(int id, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetPatientCaregiversByIdAsync(id, cancellationToken);
        if (patient == null)
        {
            throw new BadRequestException("Invalid Request.",nameof(patient));
        }

        var caregivers = patient.PatientCaregivers?.Select(pc => new CaregiverDto()
        {
            CaregiverId = pc.CaregiverId,
            FirstName = pc.Caregiver.FirstName,
            LastName = pc.Caregiver.LastName,
            Phone = pc.Caregiver.Phone,
            Email = pc.Caregiver.Email,
            OfficeName = pc.Caregiver.Office?.OfficeName ?? string.Empty,
            CreatedAt = pc.Caregiver.CreatedAt,
            IsActive = pc.Caregiver.IsActive
        }).ToList() ?? new List<CaregiverDto>();

        var patientdto= new PatientDto
        {
            PatientId = patient.PatientId,
            OfficeId = patient.OfficeId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Phone = patient.Phone,
            Email = patient.Email,
            IsActive = patient.IsActive,
            Caregivers = caregivers
        };

        return patientdto;
    }

    public async Task<ResponseMessage<bool>> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            OfficeId = dto.OfficeId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Phone = dto.Phone,
            Email = dto.Email,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        if (dto.Caregivers.Any())
        {
            var patientCaregivers = dto.Caregivers.Select(id => new PatientCaregiver
            {
                CaregiverId = id,
                AssignedAt = DateTime.UtcNow
            }).ToList();

            if (patientCaregivers.Count > 0)
                patient.PatientCaregivers = patientCaregivers;
        }
        await _repository.AddAsync(patient, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new ResponseMessage<bool>
        {
            Result = true,
            Message = "Patient created successfully.",
        };
    }

    public async Task<ResponseMessage<bool>> UpdateAsync(int id,UpdatePatientDto dto,CancellationToken cancellationToken)
    {
        
        // Load patient with existing caregivers
        var existing = await _repository.GetByIdAsync(id, cancellationToken);

        if (existing == null)
            return new ResponseMessage<bool>
            {
                Result = false,
                Message = "Patient not found."
            };

        // Update patient basic fields
        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.Phone = dto.Phone;
        existing.Email = dto.Email;
        existing.DateOfBirth = dto.DateOfBirth;
        existing.OfficeId = dto.OfficeId;
        existing.IsActive = dto.IsActive;

        // -----------------------------
        // Sync Caregivers
        // -----------------------------
        var existingCaregiverIds = existing.PatientCaregivers
            .Select(pc => pc.CaregiverId)
            .ToList();

        var newCaregiverIds = dto.Caregivers ?? new List<int>();

        // Caregivers to remove
        var caregiversToRemove = existing.PatientCaregivers
            .Where(pc => !newCaregiverIds.Contains(pc.CaregiverId))
            .ToList();

        foreach (var pc in caregiversToRemove)
        {
            existing.PatientCaregivers.Remove(pc);
        }

        // Caregivers to add
        var caregiversToAdd = newCaregiverIds
            .Where(id => !existingCaregiverIds.Contains(id))
            .Select(id => new PatientCaregiver
            {
                CaregiverId = id,
                AssignedAt = DateTime.UtcNow
            })
            .ToList();

        foreach (var pc in caregiversToAdd)
        {
            existing.PatientCaregivers.Add(pc);
        }

        // Persist changes
        _repository.Update(existing);
        await _repository.SaveChangesAsync(cancellationToken);

        return new ResponseMessage<bool>
        {
            Result = true,
            Message = "Patient Saved Successfully."
        };

    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(id, cancellationToken);
        if (patient == null) return false;

        _repository.Delete(patient);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }

}


