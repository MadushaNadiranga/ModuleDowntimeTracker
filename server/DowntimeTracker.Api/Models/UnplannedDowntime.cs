namespace DowntimeTracker.Api.Models;

public class UnplannedDowntime
{
    public int Id { get; set; }
    public int HeadId { get; set; }
    public Head? Head { get; set; }
    public string Style { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int ReasonId { get; set; }
    public Reason? Reason { get; set; }
    public string? Comments { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; } = "Running"; // Running | Stopped | Saved
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}