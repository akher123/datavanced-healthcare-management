using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Services.DTO;

public class RegisterRequest
{
    public string Username { get; set; }

    public string Password { get; set; }
    public string Email { get; set; }
    public int OfficeId { get; set; }
    public SystemRole Role { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
public class AuthResponse
{
    public string Token { get; set; }
    public string Username { get; set; }
    public IEnumerable<string> Roles { get; set; }
}