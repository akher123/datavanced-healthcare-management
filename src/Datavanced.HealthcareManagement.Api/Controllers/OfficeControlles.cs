using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Shared;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[Route(RoutePrefix.Offices)]
public class OfficeController(IOfficeService service) : BaseApiController<OfficeController>
{
    private readonly IOfficeService _service = service;

    [HttpGet("get-offices")]
    public async Task<ActionResult<ResponseMessage<IEnumerable<OfficeDto>>>> SearchOfficesAsync([FromQuery] string keyword)
    {
        var offices = await _service.SearchOfficesAsync(keyword);
        return Ok(new ResponseMessage<IEnumerable<OfficeDto>>()
        {
            Result = offices,
        });
       
    }
}
