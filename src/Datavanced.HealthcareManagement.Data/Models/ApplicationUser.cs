using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
namespace Datavanced.HealthcareManagement.Data.Models;

public class ApplicationUser : IdentityUser
{
    public int OfficeId { get; set; }        
    public Office Office { get; set; }        

}
