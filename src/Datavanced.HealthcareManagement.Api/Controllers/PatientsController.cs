
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Patients)]
public class PatientsController : BaseApiController<PatientsController>
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    
    [HttpGet("get-patients-by-Pagination")]
    [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Doctor) + "," + nameof(SystemRole.Nurse) + "," + nameof(SystemRole.Receptionist))]
    public async Task<ActionResult<ResponseMessage<PaginationResult<PatientDto>>>>GetPatientsAsync([FromQuery] PaginationRequest query, CancellationToken cancellationToken)
    {
        var result = await _service.GetFilteredAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("get-patient-by-id/{id:int}")]
    [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Doctor) + "," + nameof(SystemRole.Receptionist))]
    public async Task<ActionResult<ResponseMessage<PatientDto>>>GetPatientByIdAsync(int id,CancellationToken cancellationToken)
    {
        var patient = await _service.GetPatientByIdAsync(id, cancellationToken);
        
        return Ok(new ResponseMessage<PatientDto>
        {
            Result = patient
        });
    }

    [HttpPost("create-patient")]
    [ModelValidation]
    [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Doctor) + "," + nameof(SystemRole.Receptionist))]
    public async Task<ActionResult<ResponseMessage<bool>>>CreatePatientAsync([FromBody] CreatePatientDto dto,CancellationToken cancellationToken)
    {
        dto.OfficeId= LoggedInUser.OfficeId; 

        var result = await _service.CreateAsync(dto, cancellationToken);

        return Ok(result);
    }

    [HttpPut("update-patient/{id:int}")]
    [ModelValidation]
    [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Doctor) + "," + nameof(SystemRole.Receptionist))]
    public async Task<IActionResult>UpdatePatientAsync(int id,[FromBody] UpdatePatientDto dto,CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("delete-patient-by-id/{id:int}")]
    [Authorize(Roles = nameof(SystemRole.Admin))]
    public async Task<IActionResult>DeletePatientAsync(int id,CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);

        return Ok(new ResponseMessage<bool>
        {
            Result = result
        });
    }

}
