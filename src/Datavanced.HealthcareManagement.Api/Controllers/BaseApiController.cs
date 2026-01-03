
namespace Datavanced.HealthcareManagement.Api.Controllers;

[ValidationFilter]
public abstract class BaseApiController<T> : ControllerBase
{
    public ILoggedInUser LoggedInUser
    {
        get
        {
            ILoggedInUserService loggedInUserService = HttpContext.RequestServices.GetService<ILoggedInUserService>() as ILoggedInUserService;
            return loggedInUserService.LoggedInUser;
        }
    }
}
