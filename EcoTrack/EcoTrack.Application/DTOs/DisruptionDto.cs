namespace EcoTrack.Application.DTOs;

public record DisruptionDto(
    Guid Id,
    Guid SupplyNodeId,
    string Type,
    string Description,
    int SeverityLevel,
    bool IsActive,
    DateTime ReportedAt
);