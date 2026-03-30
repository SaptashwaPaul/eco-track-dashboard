namespace EcoTrack.Domain.Entities;

public class Supplier
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Region { get; set; }

    // Used by the disruption engine to weight probability of failure
    public int ReliabilityScore { get; set; } = 100;
}