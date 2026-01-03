using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Datavanced.HealthcareManagement.Services;

public class LoggedInUserService : ILoggedInUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly Lazy<ILoggedInUser> _lazyUser;

    public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        // Lazy initialization ensures thread safety
        _lazyUser = new Lazy<ILoggedInUser>(GetLoggedInUser, true);
    }

    public ILoggedInUser LoggedInUser => _lazyUser.Value;

    private ILoggedInUser GetLoggedInUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity?.IsAuthenticated==true)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        string userId = GetClaimValue(user, ClaimTypes.NameIdentifier);
        string officeIdStr = GetClaimValue(user, "OfficeId");

        if (!int.TryParse(officeIdStr, out int officeId))
        {
            throw new UnauthorizedAccessException("Invalid OfficeId claim.");
        }

        return new LoggedInUser
        {
            UserId = userId,
            OfficeId = officeId
        };
    }

    private static string GetClaimValue(ClaimsPrincipal user, string claimType)
    {
        var claim = user.Claims.FirstOrDefault(c => c.Type == claimType);

        if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
        {
            throw new UnauthorizedAccessException($"Missing or empty claim: {claimType}");
        }

        return claim.Value;
    }
}
