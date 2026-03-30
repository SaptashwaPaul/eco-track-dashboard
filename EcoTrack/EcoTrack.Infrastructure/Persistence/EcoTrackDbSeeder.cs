using EcoTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcoTrack.Infrastructure.Persistence;

public static class EcoTrackDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EcoTrackDbContext>();

        // If we already have products, don't seed again
        if (await context.Products.AnyAsync()) return;

        // --- 1. CREATE PRODUCTS ---
        var smartphone = new Product { Name = "EcoPhone X", Description = "Sustainably sourced smartphone.", BaseCarbonFootprintKg = 55.5m };
        var evBattery = new Product { Name = "EV PowerCell 9000", Description = "High-capacity lithium-ion vehicle battery.", BaseCarbonFootprintKg = 450.0m };
        var solarPanel = new Product { Name = "SunCatch Pro Array", Description = "Commercial grade photovoltaic array.", BaseCarbonFootprintKg = 120.5m };
        context.Products.AddRange(smartphone, evBattery, solarPanel);

        // --- 2. CREATE SUPPLIERS ---
        var suppliers = new List<Supplier>
        {
            new() { Name = "Katanga Cobalt Solutions", Region = "DRC", ReliabilityScore = 65 },
            new() { Name = "Taipei Silicon Fab", Region = "Taiwan", ReliabilityScore = 98 },
            new() { Name = "Shenzhen Assembly Partners", Region = "China", ReliabilityScore = 85 },
            new() { Name = "Atacama Lithium Corp", Region = "Chile", ReliabilityScore = 72 },
            new() { Name = "Seoul Battery Tech", Region = "South Korea", ReliabilityScore = 95 },
            new() { Name = "Munich Auto Works", Region = "Germany", ReliabilityScore = 99 },
            new() { Name = "Nevada Polysilicon", Region = "USA", ReliabilityScore = 90 },
            new() { Name = "Penang Wafer Co", Region = "Malaysia", ReliabilityScore = 88 },
            new() { Name = "Gujarat Solar Assembly", Region = "India", ReliabilityScore = 82 }
        };
        context.Suppliers.AddRange(suppliers);

        // Save to generate the unique GUIDs needed for the nodes
        await context.SaveChangesAsync();

        // --- 3. MAP THE GLOBAL SUPPLY CHAINS ---
        var nodes = new List<SupplyNode>
        {
            // Product 1: EcoPhone X (Africa -> Asia)
            new() { ProductId = smartphone.Id, SupplierId = suppliers[0].Id, NodeType = "Extraction", SequenceOrder = 1, Latitude = -10.7061, Longitude = 26.3411 },
            new() { ProductId = smartphone.Id, SupplierId = suppliers[1].Id, NodeType = "Manufacturing", SequenceOrder = 2, Latitude = 24.8066, Longitude = 120.9686 },
            new() { ProductId = smartphone.Id, SupplierId = suppliers[2].Id, NodeType = "Assembly", SequenceOrder = 3, Latitude = 22.5431, Longitude = 114.0579 },

            // Product 2: EV Battery (South America -> Asia -> Europe)
            new() { ProductId = evBattery.Id, SupplierId = suppliers[3].Id, NodeType = "Lithium Mining", SequenceOrder = 1, Latitude = -23.8634, Longitude = -69.1328 },
            new() { ProductId = evBattery.Id, SupplierId = suppliers[4].Id, NodeType = "Cell Production", SequenceOrder = 2, Latitude = 37.5665, Longitude = 126.9780 },
            new() { ProductId = evBattery.Id, SupplierId = suppliers[5].Id, NodeType = "Pack Assembly", SequenceOrder = 3, Latitude = 48.1351, Longitude = 11.5820 },

            // Product 3: Solar Array (North America -> SE Asia -> South Asia)
            new() { ProductId = solarPanel.Id, SupplierId = suppliers[6].Id, NodeType = "Silicon Refining", SequenceOrder = 1, Latitude = 38.8026, Longitude = -116.4194 },
            new() { ProductId = solarPanel.Id, SupplierId = suppliers[7].Id, NodeType = "Wafer Cutting", SequenceOrder = 2, Latitude = 5.4141, Longitude = 100.3288 },
            new() { ProductId = solarPanel.Id, SupplierId = suppliers[8].Id, NodeType = "Module Framing", SequenceOrder = 3, Latitude = 22.2587, Longitude = 71.1924 }
        };
        context.SupplyNodes.AddRange(nodes);

        await context.SaveChangesAsync();
    }
}