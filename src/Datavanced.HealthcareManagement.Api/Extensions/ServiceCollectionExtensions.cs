using Datavanced.HealthcareManagement.Api.Handler;
using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Datavanced.HealthcareManagement.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        return services;
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        // Seed Offices & Caregivers
        await InitialSeedData.SeedOfficesAsync(db);

        // Seed Roles
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        await InitialSeedData.SeedRolesAsync(roleManager);

        // Seed Single Admin User linked to OfficeId = 1
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await InitialSeedData.SeedUserAsync(userManager, 1);
    }
}

