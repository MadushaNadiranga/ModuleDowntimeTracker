namespace DowntimeTracker.Api.Models;

public class PlannedDowntime
{
    public int Id { get; set; }
    public int HeadId { get; set; }
    public Head? Head { get; set; }
    public string Type { get; set; } = string.Empty; // "Mold Change" | "Cleaning"
    public DateOnly Date { get; set; }
    public TimeOnly PlannedStartTime { get; set; }
    public TimeOnly PlannedEndTime { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public string Status { get; set; } = "Pending"; // Pending | InProgress | Completed
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}