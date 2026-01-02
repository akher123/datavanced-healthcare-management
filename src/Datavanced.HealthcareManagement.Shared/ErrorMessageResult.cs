using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared;

public class ErrorMessageResult
{
    private string _errorMessage;
    public bool IsError { get; private set; }
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            IsError = false;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            _errorMessage = value;
            IsError = true;
        }
    }
}    
