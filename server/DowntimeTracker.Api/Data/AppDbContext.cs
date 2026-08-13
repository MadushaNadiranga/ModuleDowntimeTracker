using Microsoft.EntityFrameworkCore;
using DowntimeTracker.Api.Models;

namespace DowntimeTracker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Head> Heads => Set<Head>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Reason> Reasons => Set<Reason>();
    public DbSet<PlannedDowntime> PlannedDowntimes => Set<PlannedDowntime>();
    public DbSet<PlannedDowntimeHistory> PlannedDowntimeHistories => Set<PlannedDowntimeHistory>();
    public DbSet<UnplannedDowntime> UnplannedDowntimes => Set<UnplannedDowntime>();
    public DbSet<UnplannedDowntimeHistory> UnplannedDowntimeHistories => Set<UnplannedDowntimeHistory>();
}