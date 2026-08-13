namespace DowntimeTracker.Api.Models;

public class Reason
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string ReasonText { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}