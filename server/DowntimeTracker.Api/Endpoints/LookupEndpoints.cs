using DowntimeTracker.Api.Data;
using DowntimeTracker.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DowntimeTracker.Api.Endpoints;

public static class LookupEndpoints
{
    public static void MapLookupEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/lookups").RequireAuthorization();

        group.MapGet("/modules", async (AppDbContext db) =>
            await db.Modules
                .Where(m => m.IsActive)
                .Select(m => new ModuleDto(m.Id, m.Name, m.Silo))
                .ToListAsync());

        group.MapGet("/heads", async (AppDbContext db, int? moduleId) =>
        {
            var query = db.Heads.Where(h => h.IsActive);
            if (moduleId.HasValue)
                query = query.Where(h => h.ModuleId == moduleId.Value);

            return await query
                .Select(h => new HeadDto(h.Id, h.Code, h.ModuleId))
                .ToListAsync();
        });

        group.MapGet("/departments", async (AppDbContext db) =>
            await db.Departments
                .Select(d => new DepartmentDto(d.Id, d.Name))
                .ToListAsync());

        group.MapGet("/reasons", async (AppDbContext db, int? departmentId) =>
        {
            var query = db.Reasons.Where(r => r.IsActive);
            if (departmentId.HasValue)
                query = query.Where(r => r.DepartmentId == departmentId.Value);

            return await query
                .Select(r => new ReasonDto(r.Id, r.ReasonText, r.DepartmentId))
                .ToListAsync();
        });
    }
}