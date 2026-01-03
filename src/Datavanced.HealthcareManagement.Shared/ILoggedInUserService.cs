using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared
{
    public interface ILoggedInUserService
    {
        ILoggedInUser LoggedInUser { get; }
    }
}
