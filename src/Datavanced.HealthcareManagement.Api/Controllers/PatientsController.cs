using Datavanced.HealthcareManagement.Api.Providers;
using Datavanced.HealthcareManagement.Shared;
using Datavanced.HealthcareManagement.Shared.Pagination;

namespace Datavanced.HealthcareManagement.Api.Controllers;

/// <summary>
/// Handles patient-related operations such as
/// listing, retrieving, creating, updating, and deleting patients.
/// </summary>
[Route(RoutePrefix.Patients)]
public class PatientsController : BaseApiController<PatientsController>
{
    private readonly IPatientService _service;

    /// <summary>
    /// Constructor injection of patient service.
    /// </summary>
    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of patients with optional filters.
    /// </summary>
    /// <param name="query">Pagination and filter parameters</param>
    /// <param name="cancellationToken">Request cancellation token</param>
    [HttpGet("get-patients")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<ActionResult<ResponseMessage<PaginationResult<PatientDto>>>>
        GetPatientsAsync(
            [FromQuery] PaginationRequest query,
            CancellationToken cancellationToken)
    {
        var result = await _service.GetFilteredAsync(query, cancellationToken);

        return Ok(new ResponseMessage<PaginationResult<PatientDto>>
        {
            Result = result
        });
    }

    /// <summary>
    /// Retrieves a single patient by ID.
    /// </summary>
    /// <param name="id">Patient ID</param>
    [HttpGet("get-patient-by-id/{id:int}")]
    public async Task<ActionResult<ResponseMessage<PatientDto>>>
        GetPatientByIdAsync(
            int id,
            CancellationToken cancellationToken)
    {
        var patient = await _service.GetByIdAsync(id, cancellationToken);
        
        return Ok(new ResponseMessage<PatientDto>
        {
            Result = patient
        });
    }

    /// <summary>
    /// Creates a new patient record.
    /// </summary>
    /// <param name="dto">Patient creation data</param>
    [HttpPost("create-patient")]
    [ModelValidation]
    public async Task<ActionResult<ResponseMessage<PatientDto>>>
        CreatePatientAsync(
            [FromBody] CreatePatientDto dto,
            CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);

        return Ok(new ResponseMessage<PatientDto>
        {
            Result = created
        });
    }

    /// <summary>
    /// Updates an existing patient by ID.
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="dto">Updated patient data</param>
    [HttpPut("update-patient-by-id/{id:int}")]
    [ModelValidation]
    public async Task<IActionResult>
        UpdatePatientAsync(
            int id,
            [FromBody] UpdatePatientDto dto,
            CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);

        return Ok(new ResponseMessage<bool>
        {
            Result = result
        });
    }

    /// <summary>
    /// Deletes a patient by ID.
    /// </summary>
    /// <param name="id">Patient ID</param>
    [HttpDelete("delete-patient-by-id/{id:int}")]
    public async Task<IActionResult>
        DeletePatientAsync(
            int id,
            CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);

        return Ok(new ResponseMessage<bool>
        {
            Result = result
        });
    }
}
