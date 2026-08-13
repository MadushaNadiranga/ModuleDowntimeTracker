namespace DowntimeTracker.Api.Models;

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Silo { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Head> Heads { get; set; } = new List<Head>();
}