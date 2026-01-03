using Datavanced.HealthcareManagement.Api;
using Datavanced.HealthcareManagement.Api.Extensions;
using Datavanced.HealthcareManagement.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Services

// Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();

// Application & Infrastructure
builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddApiServices();


// Authentication & Authorization
builder.Services.AddJwtAuthentication(jwtSettings);
builder.Services.AddAuthorization();


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddResponseCaching();
builder.Services.RegisterSwagger();

#endregion

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();
app.UseExceptionHandler(options => { });

#region Middleware Pipeline
// Development-only tools
if (app.Environment.IsDevelopment())
{
    // Automatically apply migrations & seed data
    await app.Services.SeedDatabaseAsync();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HCMS API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("corspolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();

app.MapControllers();

#endregion

app.Run();
