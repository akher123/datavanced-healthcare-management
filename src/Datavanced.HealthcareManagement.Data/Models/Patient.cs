using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Data.Models;

public class Patient
{
    public int PatientId { get; set; }
    public int OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public Office Office { get; set; }
    public ICollection<PatientCaregiver> PatientCaregivers { get; set; }
}
