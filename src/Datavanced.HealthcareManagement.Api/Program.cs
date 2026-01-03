using Datavanced.HealthcareManagement.Api;
using Datavanced.HealthcareManagement.Api.Extensions;
using Datavanced.HealthcareManagement.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --------------------- Configuration ---------------------
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();

// --------------------- Application & Infrastructure Services ---------------------
builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddApiServices();

// --------------------- Authentication & Authorization ---------------------
builder.Services
    .AddJwtAuthentication(jwtSettings)
    .AddAuthorization();

// --------------------- CORS ---------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// --------------------- Controllers, Caching, Swagger ---------------------
builder.Services.AddControllers();
builder.Services.AddResponseCaching();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --------------------- Exception Middleware ---------------------
app.UseMiddleware<ExceptionMiddleware>();
app.UseExceptionHandler(options => { });

// --------------------- Development-only ---------------------
if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDatabaseAsync(); // Seed DB if needed

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HCMS API v1");
        c.RoutePrefix = string.Empty; // Swagger at root
    });
}

// --------------------- Static Files & Default SPA ---------------------
app.UseDefaultFiles();  // looks for index.html
app.UseStaticFiles();   // serves wwwroot content

// --------------------- Security ---------------------
app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseAuthentication();
app.UseAuthorization();

// --------------------- Response Caching ---------------------
app.UseResponseCaching();

// --------------------- Endpoints ---------------------
app.MapControllers();

app.Run();
