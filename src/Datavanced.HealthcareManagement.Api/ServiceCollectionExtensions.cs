using Datavanced.HealthcareManagement.Api.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Datavanced.HealthcareManagement.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = async context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        await WriteResponseAsync(context.Response, HttpStatusCode.Unauthorized, "The token has expired.");
                    }
                    else
                    {
                        await WriteResponseAsync(context.Response, HttpStatusCode.InternalServerError, "An unhandled error has occurred.");
                    }
                },

                OnChallenge = async context =>
                {
                    context.HandleResponse(); // Prevent default 401 response
                    if (!context.Response.HasStarted)
                    {
                        await WriteResponseAsync(context.Response, HttpStatusCode.Unauthorized, "You are not authorized.");
                    }
                },

                OnForbidden = async context =>
                {
                    await WriteResponseAsync(context.Response, HttpStatusCode.Forbidden, "You are not authorized to access this resource.");
                }
            };
        });

        return services;
    }

    private static Task WriteResponseAsync(Microsoft.AspNetCore.Http.HttpResponse response, HttpStatusCode statusCode, string message)
    {
        response.StatusCode = (int)statusCode;
        response.ContentType = "application/json";

        var payload = System.Text.Json.JsonSerializer.Serialize(new { error = message }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return response.WriteAsync(payload);
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        return services;
    }
}
