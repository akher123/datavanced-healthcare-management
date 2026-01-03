using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Datavanced.HealthcareManagement.Api.Extensions
{
    public static class JwtServiceExtensions
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
                options.TokenValidationParameters = GetTokenValidationParameters(jwtSettings);
                options.Events = GetJwtBearerEvents();
            });

            return services;
        }

        private static TokenValidationParameters GetTokenValidationParameters(JwtSettings jwtSettings)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };
        }

        private static JwtBearerEvents GetJwtBearerEvents()
        {
            return new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        return WriteErrorResponse(context.Response, HttpStatusCode.Unauthorized, "The token has expired.");
                    }

                    return WriteErrorResponse(context.Response, HttpStatusCode.InternalServerError, "An unhandled error has occurred.");
                },

                OnChallenge = context =>
                {
                    context.HandleResponse();

                    if (!context.Response.HasStarted)
                    {
                        return WriteErrorResponse(context.Response, HttpStatusCode.Unauthorized, "You are not authorized.");
                    }

                    return Task.CompletedTask;
                },

                OnForbidden = context =>
                {
                    return WriteErrorResponse(context.Response, HttpStatusCode.Forbidden, "You are not authorized to access this resource.");
                }
            };
        }

        private static Task WriteErrorResponse(HttpResponse response, HttpStatusCode statusCode, string message)
        {
            var error = new ErrorMessage
            {
                Message = message,
                StatusCode = (int)statusCode
            };

            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            var json = JsonConvert.SerializeObject(error);
            return response.WriteAsync(json);
        }
    }
}
