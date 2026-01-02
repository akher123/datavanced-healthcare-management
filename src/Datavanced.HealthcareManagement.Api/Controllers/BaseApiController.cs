using Microsoft.AspNetCore.Authorization;

namespace Datavanced.HealthcareManagement.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
public abstract class BaseApiController<T> : ControllerBase
{

}
