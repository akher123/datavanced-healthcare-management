using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Data.Repository;
using Datavanced.HealthcareManagement.Services.DTO;
using Datavanced.HealthcareManagement.Shared.ExceptionHelper;
using Datavanced.HealthcareManagement.Shared.Pagination;

namespace Datavanced.HealthcareManagement.Services;

public interface IPatientService
{
    Task<PaginationResult<PatientDto>> GetFilteredAsync(PaginationRequest query, CancellationToken cancellationToken);
    Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PatientDto> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    Task<int> AddPatientCaregiverAsync(PatientCaregiverAssignmentDto dto, CancellationToken cancellationToken);

}

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;


    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }
    public async Task<PaginationResult<PatientDto>> GetFilteredAsync(PaginationRequest query, CancellationToken cancellationToken)
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

        return new PaginationResult<PatientDto>(patients.PageIndex, patients.PageSize, patients.TotalCount, dtos);
    }

    public async Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(id, cancellationToken);
        if (patient == null)
        {
            throw new BadRequestException("Invalid Request.",nameof(patient));
        }

        return new PatientDto
        {
            PatientId = patient.PatientId,
            OfficeId = patient.OfficeId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Phone = patient.Phone,
            Email = patient.Email,
            IsActive = patient.IsActive
        };
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken)
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

        await _repository.AddAsync(patient, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new PatientDto
        {
            PatientId = patient.PatientId,
            OfficeId = patient.OfficeId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Phone = patient.Phone,
            Email = patient.Email,
            IsActive = patient.IsActive
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing == null) return false;

        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.Phone = dto.Phone;
        existing.Email = dto.Email;
        existing.DateOfBirth = dto.DateOfBirth;
        existing.OfficeId = dto.OfficeId;
        existing.IsActive = dto.IsActive;

        _repository.Update(existing);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(id, cancellationToken);
        if (patient == null) return false;

        _repository.Delete(patient);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }


    public async Task<int> AddPatientCaregiverAsync(PatientCaregiverAssignmentDto dto, CancellationToken cancellationToken)
    {
        var assignments = dto.CaregiverIds.Select(id => new PatientCaregiver
        {
            PatientId = dto.PatientId,
            CaregiverId = id,
            AssignedAt = DateTime.UtcNow
        }).ToList();

        return await _repository.AddPatientCaregiverAsync(assignments, cancellationToken);
    }
}


