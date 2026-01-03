using Datavanced.HealthcareManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Services.DTO;

public class CreateCaregiverDto
{
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
public class UpdateCaregiverDto
{
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
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
