using Datavanced.HealthcareManagement.Shared;
using Datavanced.HealthcareManagement.Shared.Pagination;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Patients)]
public class PatientsController : BaseApiController<PatientsController>
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet("get-patients")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<ActionResult> GetPatientsAsync([FromQuery] PaginationRequest query,CancellationToken cancellationToken)
    {
        var result = await _service.GetFilteredAsync(query, cancellationToken);
        return Ok(result);
    }


    [HttpGet("get-patient-by-id/{id:int}")]
    public async Task<ActionResult<PatientDto>> GetPatientByIdAsync(int id, CancellationToken cancellationToken)
    {
        var patient = await _service.GetByIdAsync(id, cancellationToken);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPost("create-patient")]
    public async Task<ActionResult<PatientDto>> CreatePatientAsync(CreatePatientDto dto,CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetPatientByIdAsync), new { id = created.PatientId }, created);
    }

    [HttpPut("update-patient/{id:int}")]
    public async Task<IActionResult> UpdatePatientAsync( int id, UpdatePatientDto dto,CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("delete-patient/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
