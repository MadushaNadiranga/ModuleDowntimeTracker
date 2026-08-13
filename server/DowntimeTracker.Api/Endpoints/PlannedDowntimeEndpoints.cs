using DowntimeTracker.Api.Data;
using DowntimeTracker.Api.DTOs;
using DowntimeTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DowntimeTracker.Api.Endpoints;

public static class PlannedDowntimeEndpoints
{
    public static void MapPlannedDowntimeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/planned-downtime").RequireAuthorization();

        // GET list, filtered by type, only Pending records
        group.MapGet("/", async (AppDbContext db, string type) =>
        {
            var items = await db.PlannedDowntimes
                .Include(p => p.Head).ThenInclude(h => h!.Module)
                .Where(p => p.Type == type && p.Status == "Pending")
                .Select(p => new PlannedDowntimeListItemDto(
                    p.Id, p.Head!.Module!.Name, p.Head.Code, p.Date,
                    p.PlannedStartTime, p.PlannedEndTime))
                .ToListAsync();

            return Results.Ok(items);
        });

        // GET single record detail
        group.MapGet("/{id:int}", async (AppDbContext db, int id) =>
        {
            var p = await db.PlannedDowntimes
                .Include(x => x.Head).ThenInclude(h => h!.Module)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p is null) return Results.NotFound();

            var dto = new PlannedDowntimeDetailDto(
                p.Id, p.Type, p.Head!.Module!.Name, p.Head.Code, p.Date,
                p.PlannedStartTime, p.PlannedEndTime,
                p.ActualStartTime, p.ActualEndTime, p.Status);

            return Results.Ok(dto);
        });

        // PATCH start
        group.MapPatch("/{id:int}/start", async (AppDbContext db, int id) =>
        {
            var p = await db.PlannedDowntimes.FindAsync(id);
            if (p is null) return Results.NotFound();

            p.ActualStartTime = DateTime.UtcNow;
            p.Status = "InProgress";
            p.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.Ok(new StartPlannedDowntimeResponse(p.Id, p.ActualStartTime.Value, p.Status));
        });

        // PATCH end
        group.MapPatch("/{id:int}/end", async (AppDbContext db, int id) =>
        {
            var p = await db.PlannedDowntimes.FindAsync(id);
            if (p is null) return Results.NotFound();
            if (p.ActualStartTime is null) return Results.BadRequest("Cannot end before starting.");

            p.ActualEndTime = DateTime.UtcNow;
            p.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.Ok(new EndPlannedDowntimeResponse(p.Id, p.ActualEndTime.Value));
        });

        // POST save -> move active record into history, in a transaction
        group.MapPost("/{id:int}/save", async (AppDbContext db, int id, HttpContext http) =>
        {
            var p = await db.PlannedDowntimes.FindAsync(id);
            if (p is null) return Results.NotFound();
            if (p.ActualStartTime is null || p.ActualEndTime is null)
                return Results.BadRequest("Record must have both Actual Start and End times before saving.");

            var userIdClaim = http.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdClaim ?? "0");

            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var duration = (int)(p.ActualEndTime.Value - p.ActualStartTime.Value).TotalSeconds;

                var history = new PlannedDowntimeHistory
                {
                    OriginalId = p.Id,
                    HeadId = p.HeadId,
                    Type = p.Type,
                    Date = p.Date,
                    PlannedStartTime = p.PlannedStartTime,
                    PlannedEndTime = p.PlannedEndTime,
                    ActualStartTime = p.ActualStartTime.Value,
                    ActualEndTime = p.ActualEndTime.Value,
                    DurationSeconds = duration,
                    SavedBy = userId
                };

                db.PlannedDowntimeHistories.Add(history);
                db.PlannedDowntimes.Remove(p);

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.Ok(new SavePlannedDowntimeResponse(history.Id, duration));
            }
            catch
            {
                await transaction.RollbackAsync();
                return Results.Problem("Failed to save record.");
            }
        });

        // POST reset -> clear actual times without saving
        group.MapPost("/{id:int}/reset", async (AppDbContext db, int id) =>
        {
            var p = await db.PlannedDowntimes.FindAsync(id);
            if (p is null) return Results.NotFound();

            p.ActualStartTime = null;
            p.ActualEndTime = null;
            p.Status = "Pending";
            p.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.Ok();
        });
    }
}