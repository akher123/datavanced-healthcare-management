namespace Datavanced.HealthcareManagement.Services.DTO;

using System;
using System.ComponentModel.DataAnnotations;

public class CreateCaregiverDto
{
    [Required(ErrorMessage = "OfficeId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "OfficeId must be greater than 0.")]
    public int OfficeId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "CreatedAt is required.")]
    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; } = true; // default true
}

public class UpdateCaregiverDto
{
    [Required(ErrorMessage = "OfficeId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "OfficeId must be greater than 0.")]
    public int OfficeId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "CreatedAt is required.")]
    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }
}
public class CaregiverDto
{
    public int CaregiverId { get; set; }
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string OfficeName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public string FullName => $"{FirstName} {LastName}";

}
public class CaregiverSearchDto
{
    public int CaregiverId { get; set; }
    public string CaregiverName { get; set; }

}



