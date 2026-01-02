using Datavanced.HealthcareManagement.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        string userId = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        string userName = httpContext?.User.Identity?.Name;
        string officeId = httpContext?.User.FindFirst("OfficeId")?.Value;
        string ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();

        var entries = eventData.Context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added
                     || e.State == EntityState.Modified
                     || e.State == EntityState.Deleted).ToList();

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;
            var entityId = entry.Properties
                                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())
                                ?.CurrentValue?.ToString();

            var audit = new AuditEvent
            {
                UserId = userId,
                UserName = userName,
                OfficeId = officeId,
                Entity = entityName,
                EntityId = entityId,
                Action = $"{entry.State} {entityName}",
                ActionType = entry.State switch
                {
                    EntityState.Added => "CREATE",
                    EntityState.Modified => "UPDATE",
                    EntityState.Deleted => "DELETE",
                    _ => "UNKNOWN"
                },
                Timestamp = DateTime.UtcNow
            };

            await eventData.Context.Set<AuditEvent>()
                .AddAsync(audit, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
