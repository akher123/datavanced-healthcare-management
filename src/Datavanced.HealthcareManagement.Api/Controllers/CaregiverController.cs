namespace Datavanced.HealthcareManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaregiverController : ControllerBase
{
    private readonly ICaregiverService _service;

    public CaregiverController(ICaregiverService service)
    {
        _service = service;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CaregiverDto>>> Search([FromQuery] string keyword)
    {
        var caregivers = await _service.SearchCaregiversAsync(keyword);
        return Ok(caregivers);
    }
}
