namespace EcoTrack.Domain.Entities;

public class SupplyNode
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid ProductId { get; set; }
    public required Guid SupplierId { get; set; }

    // e.g., "Raw Material Extraction", "Manufacturing", "Port", "Retail"
    public required string NodeType { get; set; }

    // Coordinates for Leaflet/D3 placement
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }

    // The order of this node in the product's lifecycle
    public int SequenceOrder { get; set; }

    // Navigation properties
    public Product? Product { get; set; }
    public Supplier? Supplier { get; set; }
    public ICollection<Disruption> Disruptions { get; set; } = [];
}