namespace EcoTrack.Domain.Entities;

public class Disruption
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid SupplyNodeId { get; set; }

    // e.g., "Typhoon", "Labor Strike", "Customs Delay"
    public required string Type { get; set; }
    public required string Description { get; set; }

    // 1-10 scale; SignalR will broadcast High Severity differently
    public required int SeverityLevel { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime ReportedAt { get; init; } = DateTime.UtcNow;

    // Navigation
    public SupplyNode? SupplyNode { get; set; }
}