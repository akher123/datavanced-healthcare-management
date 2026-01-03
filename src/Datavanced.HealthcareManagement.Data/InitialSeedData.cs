namespace Datavanced.HealthcareManagement.Data;
using Datavanced.HealthcareManagement.Data.Models;
using Datavanced.HealthcareManagement.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class InitialSeedData
{
    public static async Task SeedOfficesAsync(ApplicationDbContext context)
    {
        if (context.Offices.Any()) return;

        // ----- Add Offices -----
        var dhakaClinic = new Office
        {
            OfficeName = "Dhaka Central Clinic",
            AddressLine = "12/3 Shahbagh, Dhaka-1000",
            Phone = "+880 2 9561234",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var chittagongClinic = new Office
        {
            OfficeName = "Chittagong Care Center",
            AddressLine = "45 Agrabad Commercial Area, Chittagong-4100",
            Phone = "+880 31 2456789",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var sylhetClinic = new Office
        {
            OfficeName = "Sylhet Health Hub",
            AddressLine = "67 Zindabazar, Sylhet-3100",
            Phone = "+880 821 567890",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        context.Offices.AddRange(dhakaClinic, chittagongClinic, sylhetClinic);
        context.SaveChanges(); // Save to generate OfficeIds

        // ----- Add Caregivers -----
        context.Caregivers.AddRange(
            // Dhaka Clinic
            new Caregiver
            {
                FirstName = "Rahim",
                LastName = "Ahmed",
                Email = "rahim.ahmed@dhakaclinic.bd",
                Phone = "+880 1711 123456",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OfficeId = dhakaClinic.OfficeId
            },
            new Caregiver
            {
                FirstName = "Fatema",
                LastName = "Khatun",
                Email = "fatema.khatun@dhakaclinic.bd",
                Phone = "+880 1711 234567",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OfficeId = dhakaClinic.OfficeId
            },

            // Chittagong Clinic
            new Caregiver
            {
                FirstName = "Karim",
                LastName = "Chowdhury",
                Email = "karim.chowdhury@chittagongcare.bd",
                Phone = "+880 1712 345678",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OfficeId = chittagongClinic.OfficeId
            },
            new Caregiver
            {
                FirstName = "Muna",
                LastName = "Begum",
                Email = "muna.begum@chittagongcare.bd",
                Phone = "+880 1712 456789",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OfficeId = chittagongClinic.OfficeId
            },

            // Sylhet Clinic
            new Caregiver
            {
                FirstName = "Rafiq",
                LastName = "Hossain",
                Email = "rafiq.hossain@sylhethealth.bd",
                Phone = "+880 1713 567890",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OfficeId = sylhetClinic.OfficeId
            },
            new Caregiver
            {
                FirstName = "Nadia",
                LastName = "Rahman",
                Email = "nadia.rahman@sylhethealth.bd",
                Phone = "+880 1713 678901",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OfficeId = sylhetClinic.OfficeId
            }
        );

      await  context.SaveChangesAsync();
    }

    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { nameof(SystemRole.Admin), nameof(SystemRole.Doctor), nameof(SystemRole.Nurse), nameof(SystemRole.Receptionist) };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }
    }


    public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, int officeId)
    {
        // Check if user already exists
        var existingUser = await userManager.FindByNameAsync("admin");
        if (existingUser != null) return;

        var user = new ApplicationUser
        {
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@clinic.bd",
            NormalizedEmail = "ADMIN@CLINIC.BD",
            EmailConfirmed = true,
            PhoneNumber = "+8801711000000",
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            OfficeId = officeId
        };

        // Set password (Identity hashes it)
        var result = await userManager.CreateAsync(user, "Admin@123"); 

        if (result.Succeeded)
        {
            // Assign role
            await userManager.AddToRoleAsync(user, nameof(SystemRole.Admin));

            // If your ApplicationUser has OfficeId:
            if (user is ApplicationUser appUser)
            {
                appUser.OfficeId = officeId;
                await userManager.UpdateAsync(user);
            }
        }
    }

}

