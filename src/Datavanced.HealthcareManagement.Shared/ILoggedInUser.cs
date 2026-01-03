namespace Datavanced.HealthcareManagement.Shared;

public interface ILoggedInUser
{
    string UserId { get; set; }
    int OfficeId { get; set; }
}
