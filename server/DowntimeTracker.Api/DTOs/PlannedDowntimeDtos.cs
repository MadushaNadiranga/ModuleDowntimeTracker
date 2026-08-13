namespace DowntimeTracker.Api.DTOs;

public record PlannedDowntimeListItemDto(
    int Id, string ModuleName, string HeadCode, DateOnly Date,
    TimeOnly PlannedStartTime, TimeOnly PlannedEndTime);

public record PlannedDowntimeDetailDto(
    int Id, string Type, string ModuleName, string HeadCode, DateOnly Date,
    TimeOnly PlannedStartTime, TimeOnly PlannedEndTime,
    DateTime? ActualStartTime, DateTime? ActualEndTime, string Status);

public record StartPlannedDowntimeResponse(int Id, DateTime ActualStartTime, string Status);
public record EndPlannedDowntimeResponse(int Id, DateTime ActualEndTime);
public record SavePlannedDowntimeResponse(int HistoryId, int DurationSeconds);