namespace Datavanced.HealthcareManagement.Shared.Pagination;

public class PaginationRequest
{
    public string? Keyword { get; set; } 
    public string? SortBy { get; set; } = "PatientId"; 
    public bool Descending { get; set; } = false; 
    public int PageIndex { get; set; } = 1; 
    public int PageSize { get; set; } = 10; 
}

