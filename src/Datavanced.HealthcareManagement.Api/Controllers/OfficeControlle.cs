using Datavanced.HealthcareManagement.Shared;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Offices)]
public class OfficeControlle(IOfficeService service) : BaseApiController<OfficeControlle>
{
    private readonly IOfficeService _service = service;

    [HttpGet("search-offices")]
    public async Task<ActionResult<IEnumerable<OfficeDto>>> SearchOfficesAsync([FromQuery] string keyword)
    {
        var offices = await _service.SearchOfficesAsync(keyword);
        return Ok(offices);
    }
}
