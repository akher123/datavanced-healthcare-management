using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Services.DTO;

public class PatientCaregiverAssignmentDto
{
    public int PatientId { get; set; }
    public List<int> CaregiverIds { get; set; } = new List<int>();
}
