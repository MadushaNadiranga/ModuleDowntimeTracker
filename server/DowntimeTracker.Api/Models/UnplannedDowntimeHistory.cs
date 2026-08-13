namespace DowntimeTracker.Api.Models;

public class UnplannedDowntimeHistory
{
    public int Id { get; set; }
    public int OriginalId { get; set; }
    public int HeadId { get; set; }
    public string Style { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int ReasonId { get; set; }
    public string? Comments { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int DurationSeconds { get; set; }
    public int SavedBy { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}