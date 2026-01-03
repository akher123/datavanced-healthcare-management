
using Datavanced.HealthcareManagement.Shared.Pagination;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Caregivers)]
public class CaregiverController(ICaregiverService service) : BaseApiController<CaregiverController>
{
    private readonly ICaregiverService _service = service;

    [HttpGet("get-caregivers")]
    public async Task<ActionResult<ResponseMessage<IEnumerable<CaregiverDto>> >> SearchCaregiversAsync([FromQuery] string keyword)
    {
        var caregivers = await _service.SearchCaregiversAsync(keyword);

        return Ok(new ResponseMessage<IEnumerable<CaregiverDto>>()
        {
            Result= caregivers,
        });
    }

    [HttpGet("get-caregivers-by-Pagination")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<ActionResult<ResponseMessage<PaginationResult<CaregiverDto>>>>
       GetPatientsAsync(
           [FromQuery] PaginationRequest query,
           CancellationToken cancellationToken)
    {
        var result = await _service.GetCaregiverPaginationAsync(query, cancellationToken);

        return Ok(new ResponseMessage<PaginationResult<CaregiverDto>>
        {
            Result = result
        });
    }
}


