using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Datavanced.HealthcareManagement.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices (this IServiceCollection services)
    {
        services.AddScoped<ICaregiverService,CaregiverService>();
        services.AddScoped<IOfficeService, OfficeService>();
        services.AddScoped<IPatientService, PatientService>();

        return services;
    }
}
