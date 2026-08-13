namespace DowntimeTracker.Api.Models;

public class Head
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int ModuleId { get; set; }
    public Module? Module { get; set; }
    public bool IsActive { get; set; } = true;
}