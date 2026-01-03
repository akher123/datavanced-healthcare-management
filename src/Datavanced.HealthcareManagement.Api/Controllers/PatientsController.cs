using Datavanced.HealthcareManagement.Api.Providers;
using Datavanced.HealthcareManagement.Shared;
using Datavanced.HealthcareManagement.Shared.ExceptionHelper;
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


    [HttpPost("Assign-patient-caregivers")]
    public async Task<IActionResult> AssignPatientCaregivers([FromBody] PatientCaregiverAssignmentDto dto, CancellationToken cancellationToken)
    {
        if (dto.CaregiverIds == null || !dto.CaregiverIds.Any())
        {
            throw new BadRequestException("CaregiverIds cannot be empty.", "AssignCaregivers");
        }

        var effectedRows= await _service.AddPatientCaregiverAsync(dto, cancellationToken);
        return Ok(new ResponseMessage<bool>
        {
            Result = effectedRows > 0,
            Message = $"{effectedRows} Caregivers assigned successfully." 
        });

    }
}
