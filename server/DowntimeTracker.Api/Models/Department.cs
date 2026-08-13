namespace DowntimeTracker.Api.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Reason> Reasons { get; set; } = new List<Reason>();
}