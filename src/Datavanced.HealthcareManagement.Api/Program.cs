using Datavanced.HealthcareManagement.Api;
using Datavanced.HealthcareManagement.Api.Middleware;
using Datavanced.HealthcareManagement.Shared;

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

// Controllers & API
builder.Services.AddControllers();


builder.Services.AddResponseCaching();

// Swagger / OpenAPI
builder.Services.RegisterSwagger();

#endregion

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseExceptionHandler(options => { });

#region Middleware Pipeline
// Development-only tools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HCMS API v1");
        c.RoutePrefix = string.Empty;
    });
}



// Static files
app.UseStaticFiles();

// Security
app.UseHttpsRedirection();

// CORS must be before auth
app.UseCors("corspolicy");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Caching
app.UseResponseCaching();

// Endpoints

app.MapControllers();

#endregion

app.Run();
