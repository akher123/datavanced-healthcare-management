using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Services.DTO;

using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [EnumDataType(typeof(SystemRole), ErrorMessage = "Invalid role.")]
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