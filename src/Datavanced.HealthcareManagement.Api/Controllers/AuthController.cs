using Datavanced.HealthcareManagement.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Datavanced.HealthcareManagement.Api.Controllers
{
    [Route(RoutePrefix.Auths)]
    public class AuthController: BaseApiController<AuthController>
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [Authorize(Roles = nameof(SystemRole.Admin))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            request.OfficeId=LoggedInUser.OfficeId;
            await _authService.RegisterAsync(request);
            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
    }
}
