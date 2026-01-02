using System;
using System.Collections.Generic;
using System.Text;

namespace Datavanced.HealthcareManagement.Shared.ExceptionHelper;

public class DuplicateRequestException : Exception
{
    public DuplicateRequestException(string message)
        : base(message)
    {
    }

    public DuplicateRequestException(string message, string details)
        : base(message)
    {
        Details = details;
    }

    public string? Details { get; }
}
