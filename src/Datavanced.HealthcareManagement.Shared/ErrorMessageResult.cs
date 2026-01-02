using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared;

public class ErrorMessage
{
    public bool IsError { get; private set; }

    private string message;
    public int? StatusCode { get; set; }
    public string Message
    {
        get
        {
            return message;
        }
        set
        {
            IsError = false;
            if (!string.IsNullOrWhiteSpace(value))
            {
                message = value;
                IsError = true;
            }
        }
    }
}