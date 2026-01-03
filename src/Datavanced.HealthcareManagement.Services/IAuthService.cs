using Microsoft.AspNetCore.Identity;

namespace Datavanced.HealthcareManagement.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}


public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly JwtTokenService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        JwtTokenService jwtService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = $"{request.Username}@example.com",
            OfficeId = request.OfficeId
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(string.Join(", ",
                result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = request.Role
            });
        }

        await _userManager.AddToRoleAsync(user, request.Role);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }
            
        IList<string> roles = await _userManager.GetRolesAsync(user);
        var token =  _jwtService.GenerateToken(user, roles);

        return new AuthResponse
        {
            Token = token,
            Username = user.UserName,
            Roles = roles
        };
    }
}
