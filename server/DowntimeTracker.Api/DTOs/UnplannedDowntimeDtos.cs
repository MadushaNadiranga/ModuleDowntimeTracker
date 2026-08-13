namespace DowntimeTracker.Api.DTOs;

public record StartUnplannedDowntimeRequest(
    int HeadId, string Style, string Size, int DepartmentId, int ReasonId, string? Comments);

public record UnplannedDowntimeItemDto(
    int Id, string HeadCode, string ModuleName, string Style, string Size,
    string DepartmentName, string ReasonText, string? Comments,
    DateTime StartTime, DateTime? EndTime, string Status);

public record StopUnplannedDowntimeResponse(int Id, DateTime EndTime, string Status);
public record SaveUnplannedDowntimeResponse(int HistoryId, int DurationSeconds);