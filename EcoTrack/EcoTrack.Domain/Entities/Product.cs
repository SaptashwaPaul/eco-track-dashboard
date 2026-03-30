namespace EcoTrack.Domain.Entities;


public class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Description { get; set; }

    // Baseline emissions before transit
    public decimal BaseCarbonFootprintKg { get; set; }

    // Navigation: A product has a journey (nodes)
    public ICollection<SupplyNode> SupplyChain { get; set; } = [];
}