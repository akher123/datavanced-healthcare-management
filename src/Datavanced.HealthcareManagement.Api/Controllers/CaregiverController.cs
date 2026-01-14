
namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Caregivers)]
public class CaregiverController(ICaregiverService service) : BaseApiController<CaregiverController>
{
    private readonly ICaregiverService _service = service;

    [HttpGet("get-caregivers")]
    [Authorize(Roles =  nameof(SystemRole.Admin) + ","+ nameof(SystemRole.Receptionist))]
    public async Task<ActionResult<ResponseMessage<IEnumerable<CaregiverSearchDto>> >> SearchCaregiversAsync([FromQuery] string keyword="")
    {
        

        var caregivers = await _service.SearchCaregiversAsync(keyword, LoggedInUser.OfficeId);
   
        return Ok(new ResponseMessage<IEnumerable<CaregiverSearchDto>>()
        {
            Result= caregivers,
        });
    }

    [HttpGet("get-caregivers-by-Pagination")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    [Authorize(Roles = nameof(SystemRole.Admin) + "," + nameof(SystemRole.Doctor) + "," + nameof(SystemRole.Nurse) + "," + nameof(SystemRole.Receptionist))]
    public async Task<ActionResult<ResponseMessage<PaginationResult<CaregiverDto>>>>GetPatientsAsync([FromQuery] PaginationRequest query,CancellationToken cancellationToken)
    {
        var result = await _service.GetCaregiverPaginationAsync(query, cancellationToken);
       
        return Ok(new ResponseMessage<PaginationResult<CaregiverDto>>
        {
            Result = result
        });
    }

    [HttpPost("create-caregiver")]
    [ModelValidation]
    [Authorize(Roles = nameof(SystemRole.Admin))]
    public async Task<ActionResult<ResponseMessage<CaregiverDto>>> CreateCaregiverAsync([FromBody] CreateCaregiverDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update-caregiver/{id:int}")]
    [ModelValidation]
    [Authorize(Roles = nameof(SystemRole.Admin))]
    public async Task<ActionResult<ResponseMessage<bool>>> UpdateCaregiverAsync(int id, [FromBody] UpdateCaregiverDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateCaregiverAsync(id,dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("delete-caregiver-by-id/{id:int}")]
    [Authorize(Roles = nameof(SystemRole.Admin))]
    public async Task<IActionResult> DeleteCaregiverByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteCaregiverByIdAsync(id, cancellationToken);

        return Ok(new ResponseMessage<bool>
        {
            Result = result
        });
    }
}


