using System;
using System.Collections.Generic;
using System.Text;

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
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}
public class UpdatePatientDto
{
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}
