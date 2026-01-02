using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared
{
    public class ResponseMessage<T> : ErrorMessageResult
    {
        public T Result { get; set; }
        public int Total { get; set; }
        public int StatusCode { get; set; }

    }
}
