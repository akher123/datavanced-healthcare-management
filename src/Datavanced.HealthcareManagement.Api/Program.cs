

using Datavanced.HealthcareManagement.Api;
using Datavanced.HealthcareManagement.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

builder.Services.AddCors(options => options.AddPolicy("corspolicy", build => {
    build.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();

}));
// JWT Authentication
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();
builder.Services.RegisterSwagger();
builder.Services.AddJwtAuthentication(jwtSettings);

builder.Services.AddAuthorization();


builder.Services.AddResponseCaching();

builder.Services.AddControllers();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HCMS API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();


app.MapControllers();

app.Run();
