using Microsoft.AspNetCore.Authorization;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[ApiController]
//[Authorize]
public abstract class BaseApiController<T> : ControllerBase
{

}
