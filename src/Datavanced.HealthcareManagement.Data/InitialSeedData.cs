namespace Datavanced.HealthcareManagement.Data;
using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

internal static class InitialSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // ----- Offices -----
        var offices = new List<Office>
        {
            new Office
            {
                OfficeId = 1,
                OfficeName = "Downtown Clinic",
                AddressLine = "123 Main St, Downtown",
                Phone = "555-0100",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Office
            {
                OfficeId = 2,
                OfficeName = "Uptown Clinic",
                AddressLine = "456 Elm St, Uptown",
                Phone = "555-0200",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        modelBuilder.Entity<Office>().HasData(offices);

        // ----- Caregivers -----
        var caregivers = new List<Caregiver>
        {
            new Caregiver
            {
                CaregiverId = 1,
                OfficeId = 1,
                FirstName = "Alice",
                LastName = "Smith",
                Phone = "555-1001",
                Email = "alice.smith@example.com",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Caregiver
            {
                CaregiverId = 2,
                OfficeId = 1,
                FirstName = "Bob",
                LastName = "Johnson",
                Phone = "555-1002",
                Email = "bob.johnson@example.com",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Caregiver
            {
                CaregiverId = 3,
                OfficeId = 2,
                FirstName = "Carol",
                LastName = "Williams",
                Phone = "555-2001",
                Email = "carol.williams@example.com",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        modelBuilder.Entity<Caregiver>().HasData(caregivers);
    }
}

