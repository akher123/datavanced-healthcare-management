using Datavanced.HealthcareManagement.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Datavanced.HealthcareManagement.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            options.UseSqlServer(connectionString);
        });

       services.AddScoped<ICaregiverRepository, CaregiverRepository>();

        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();

        return services;
    }
}
