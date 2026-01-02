using System;

namespace Datavanced.HealthcareManagement.Data.Models
{
    public sealed class AuditEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();  

        public string? UserId { get; set; }        
        public string? UserName { get; set; } 
        public string? OfficeId { get; set; }    
        public string Entity { get; set; } = null!;    
        public string? EntityId { get; set; }          
        public string Action { get; set; } = null!;       
        public string ActionType { get; set; } = null!;  
        public string? Details { get; set; }         

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
