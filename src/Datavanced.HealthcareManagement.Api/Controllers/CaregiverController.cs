
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
}
