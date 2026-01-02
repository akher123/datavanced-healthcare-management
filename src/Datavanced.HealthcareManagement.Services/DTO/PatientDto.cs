using System.ComponentModel.DataAnnotations;

namespace Datavanced.HealthcareManagement.Services.DTO;

// For returning data
public class PatientDto
{
    public int PatientId { get; set; }
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string OfficeName { get; set; }
    public bool IsActive { get; set; }
}

// For creating/updating data
public class CreatePatientDto
{
    [Required(ErrorMessage = "OfficeId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "OfficeId must be greater than 0")]
    public int OfficeId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string LastName { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100)]
    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdatePatientDto
{
    [Required(ErrorMessage = "OfficeId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "OfficeId must be greater than 0")]
    public int OfficeId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string LastName { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100)]
    public string? Email { get; set; }

    public bool IsActive { get; set; }
}
