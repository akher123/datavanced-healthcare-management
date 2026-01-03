namespace Datavanced.HealthcareManagement.Api.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;

public static class SwaggerExtensions
{
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            AddSwaggerDoc(c);
            AddXmlComments(c);
            AddJwtSecurity(c);
        });
    }

    private static void AddSwaggerDoc(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "HCMS ERP",
            Description = "Healthcare Management API",
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
    }

    private static void AddXmlComments(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic);

        foreach (var assembly in assemblies)
        {
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(baseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        }
    }

    private static void AddJwtSecurity(SwaggerGenOptions c)
    {
        // JWT Bearer Authentication
        c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter JWT token in the format: bearer {your token}"
        });
        c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });

    }
}


