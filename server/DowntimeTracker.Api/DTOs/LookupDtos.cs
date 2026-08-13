namespace DowntimeTracker.Api.DTOs;

public record ModuleDto(int Id, string Name, string? Silo);
public record HeadDto(int Id, string Code, int ModuleId);
public record DepartmentDto(int Id, string Name);
public record ReasonDto(int Id, string ReasonText, int DepartmentId);