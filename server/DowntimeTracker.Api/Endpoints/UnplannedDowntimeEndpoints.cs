using DowntimeTracker.Api.Data;
using DowntimeTracker.Api.DTOs;
using DowntimeTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DowntimeTracker.Api.Endpoints;

public static class UnplannedDowntimeEndpoints
{
    public static void MapUnplannedDowntimeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/unplanned-downtime").RequireAuthorization();

        // POST start -> create a new active timer row (no limit per Head)
        group.MapPost("/start", async (AppDbContext db, StartUnplannedDowntimeRequest req, HttpContext http) =>
        {
            var userIdClaim = http.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim ?? "0");

            var record = new UnplannedDowntime
            {
                HeadId = req.HeadId,
                Style = req.Style,
                Size = req.Size,
                DepartmentId = req.DepartmentId,
                ReasonId = req.ReasonId,
                Comments = req.Comments,
                StartTime = DateTime.UtcNow,
                Status = "Running",
                CreatedBy = userId
            };

            db.UnplannedDowntimes.Add(record);
            await db.SaveChangesAsync();

            // Reload with navigation properties for the response
            var loaded = await db.UnplannedDowntimes
                .Include(u => u.Head).ThenInclude(h => h!.Module)
                .Include(u => u.Department)
                .Include(u => u.Reason)
                .FirstAsync(u => u.Id == record.Id);

            var dto = ToDto(loaded);
            return Results.Ok(dto);
        });

        // GET active list, optionally filtered by module
        group.MapGet("/active", async (AppDbContext db, int? moduleId) =>
        {
            var query = db.UnplannedDowntimes
                .Include(u => u.Head).ThenInclude(h => h!.Module)
                .Include(u => u.Department)
                .Include(u => u.Reason)
                .Where(u => u.Status == "Running" || u.Status == "Stopped");

            if (moduleId.HasValue)
                query = query.Where(u => u.Head!.ModuleId == moduleId.Value);

            var items = await query.ToListAsync();
            return Results.Ok(items.Select(ToDto));
        });

        // PATCH stop
        group.MapPatch("/{id:int}/stop", async (AppDbContext db, int id) =>
        {
            var u = await db.UnplannedDowntimes.FindAsync(id);
            if (u is null) return Results.NotFound();
            if (u.Status != "Running") return Results.BadRequest("Only a running timer can be stopped.");

            u.EndTime = DateTime.UtcNow;
            u.Status = "Stopped";
            await db.SaveChangesAsync();

            return Results.Ok(new StopUnplannedDowntimeResponse(u.Id, u.EndTime.Value, u.Status));
        });

        // POST save -> move to history, in a transaction
        group.MapPost("/{id:int}/save", async (AppDbContext db, int id, HttpContext http) =>
        {
            var u = await db.UnplannedDowntimes.FindAsync(id);
            if (u is null) return Results.NotFound();
            if (u.Status != "Stopped" || u.EndTime is null)
                return Results.BadRequest("Timer must be stopped before saving.");

            var userIdClaim = http.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim ?? "0");

            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var duration = (int)(u.EndTime.Value - u.StartTime).TotalSeconds;

                var history = new UnplannedDowntimeHistory
                {
                    OriginalId = u.Id,
                    HeadId = u.HeadId,
                    Style = u.Style,
                    Size = u.Size,
                    DepartmentId = u.DepartmentId,
                    ReasonId = u.ReasonId,
                    Comments = u.Comments,
                    StartTime = u.StartTime,
                    EndTime = u.EndTime.Value,
                    DurationSeconds = duration,
                    SavedBy = userId
                };

                db.UnplannedDowntimeHistories.Add(history);
                db.UnplannedDowntimes.Remove(u);

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.Ok(new SaveUnplannedDowntimeResponse(history.Id, duration));
            }
            catch
            {
                await transaction.RollbackAsync();
                return Results.Problem("Failed to save record.");
            }
        });
    }

    private static UnplannedDowntimeItemDto ToDto(UnplannedDowntime u) => new(
        u.Id, u.Head!.Code, u.Head.Module!.Name, u.Style, u.Size,
        u.Department!.Name, u.Reason!.ReasonText, u.Comments,
        u.StartTime, u.EndTime, u.Status);
}