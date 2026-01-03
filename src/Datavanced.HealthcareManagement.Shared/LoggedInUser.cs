using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared
{
    public class LoggedInUser : ILoggedInUser
    {
        public string UserId { get; set; }
        public int OfficeId { get; set; }
    }
}
