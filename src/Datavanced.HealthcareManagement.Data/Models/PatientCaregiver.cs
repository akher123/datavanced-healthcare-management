using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Data.Models;

public class PatientCaregiver
{
    public int PatientId { get; set; }
    public int CaregiverId { get; set; }
    public DateTime AssignedAt { get; set; }

    public Patient Patient { get; set; }
    public Caregiver Caregiver { get; set; }
}
