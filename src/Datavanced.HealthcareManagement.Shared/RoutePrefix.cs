using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared
{
    public class RoutePrefix
    {
        private const string RoutePrefixBase = ApiRoutePrefix.RoutePrefixBase + "hcms/";
        public const string Offices = RoutePrefixBase + "offices";
        public const string Caregivers = RoutePrefixBase + "caregivers";
        public const string Patients = RoutePrefixBase + "patients";
        public const string Auths = RoutePrefixBase + "auth";
        
    }
}
