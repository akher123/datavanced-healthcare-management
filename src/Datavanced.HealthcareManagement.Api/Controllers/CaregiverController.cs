using Datavanced.HealthcareManagement.Shared;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Caregivers)]
public class CaregiverController(ICaregiverService service) : BaseApiController<CaregiverController>
{
    private readonly ICaregiverService _service = service;

    [HttpGet("search-caregivers")]
    public async Task<ActionResult<IEnumerable<CaregiverDto>>> SearchCaregiversAsync([FromQuery] string keyword)
    {
        var caregivers = await _service.SearchCaregiversAsync(keyword);
        return Ok(caregivers);
    }
}
