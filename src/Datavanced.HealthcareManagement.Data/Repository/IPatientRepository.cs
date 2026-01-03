using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datavanced.HealthcareManagement.Data.Repository;

public interface IPatientRepository
{
    Task<PaginationResult<Patient>> GetFilteredAsync( PaginationRequest query,CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Patient?> GetPatientCaregiversByIdAsync(int patientId, CancellationToken cancellationToken);
    Task AddAsync(Patient patient, CancellationToken cancellationToken);
    void Update(Patient patient);
    void Delete(Patient patient);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}


public class PatientRepository : IPatientRepository
{
    private readonly IApplicationDbContext _context;

    public PatientRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResult<Patient>> GetFilteredAsync(PaginationRequest query, CancellationToken cancellationToken)
    {
        int pageIndex = query.PageIndex;
        int pageSize = query.PageSize;

        var patientsQuery = _context.Patients
            .Include(p => p.Office)
            .Include(p => p.PatientCaregivers)
                .ThenInclude(pc => pc.Caregiver)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = $"%{query.Keyword.Trim()}%";

            patientsQuery = patientsQuery.Where(p =>
                EF.Functions.Like(p.FirstName, keyword) ||
                EF.Functions.Like(p.LastName, keyword) ||
                EF.Functions.Like(p.Email, keyword) ||
                EF.Functions.Like(p.Phone, keyword) ||
                EF.Functions.Like(p.Office.OfficeName, keyword) ||
                p.PatientCaregivers.Any(pc =>
                    EF.Functions.Like(pc.Caregiver.FirstName, keyword) ||
                    EF.Functions.Like(pc.Caregiver.LastName, keyword))
            );
        }

        switch (query.SortBy?.ToLower())
        {
            case "firstname":
                patientsQuery = query.Descending ? patientsQuery.OrderByDescending(p => p.FirstName) : patientsQuery.OrderBy(p => p.FirstName);
                break;
            case "lastname":
                patientsQuery = query.Descending ? patientsQuery.OrderByDescending(p => p.LastName) : patientsQuery.OrderBy(p => p.LastName);
                break;
            case "dateofbirth":
                patientsQuery = query.Descending ? patientsQuery.OrderByDescending(p => p.DateOfBirth) : patientsQuery.OrderBy(p => p.DateOfBirth);
                break;
            case "office":
                patientsQuery = query.Descending ? patientsQuery.OrderByDescending(p => p.Office.OfficeName) : patientsQuery.OrderBy(p => p.Office.OfficeName);
                break;
            default:
                patientsQuery = query.Descending ? patientsQuery.OrderByDescending(p => p.PatientId) : patientsQuery.OrderBy(p => p.PatientId);
                break;
        }

        var total = await patientsQuery.CountAsync(cancellationToken);

        var patients = await patientsQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginationResult<Patient>(pageIndex, pageSize, total, patients);
    }

    public async Task<Patient?> GetPatientCaregiversByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Patients
            .Include(p => p.PatientCaregivers)
                .ThenInclude(pc => pc.Caregiver)
            .FirstOrDefaultAsync(p => p.PatientId == id, cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync (int id, CancellationToken cancellationToken)
    {
        return await _context.Patients
                .Include(p => p.PatientCaregivers)
                .FirstOrDefaultAsync(p => p.PatientId == id, cancellationToken);
    }

    public async Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
    }

    public void Update(Patient patient)
    {
        _context.Patients.Update(patient);
    }

    public void Delete(Patient patient)
    {
        _context.Patients.Remove(patient);
    }
  

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

