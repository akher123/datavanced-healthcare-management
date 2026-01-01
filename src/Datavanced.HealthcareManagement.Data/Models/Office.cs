using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Data.Models;

public class Office
{
    public int OfficeId { get; set; }
    public string OfficeName { get; set; }
    public string AddressLine { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Caregiver> Caregivers { get; set; }
    public ICollection<Patient> Patients { get; set; }
}
