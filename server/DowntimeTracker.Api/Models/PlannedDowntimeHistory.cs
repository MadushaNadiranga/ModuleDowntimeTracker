namespace DowntimeTracker.Api.Models;

public class PlannedDowntimeHistory
{
    public int Id { get; set; }
    public int OriginalId { get; set; }
    public int HeadId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly PlannedStartTime { get; set; }
    public TimeOnly PlannedEndTime { get; set; }
    public DateTime ActualStartTime { get; set; }
    public DateTime ActualEndTime { get; set; }
    public int DurationSeconds { get; set; }
    public int SavedBy { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}