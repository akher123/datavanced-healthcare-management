using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Services.DTO
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
