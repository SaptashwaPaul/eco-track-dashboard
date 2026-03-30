namespace EcoTrack.Application.DTOs;

public record SupplyNodeDto(
    Guid Id,
    string ProductName,
    string SupplierName,
    string NodeType,
    double Latitude,
    double Longitude,
    int SequenceOrder
);