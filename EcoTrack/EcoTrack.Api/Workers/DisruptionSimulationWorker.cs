using EcoTrack.Api.Hubs;
using EcoTrack.Domain.Entities;
using EcoTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EcoTrack.Api.Workers;

public class DisruptionSimulationWorker(
    IServiceProvider serviceProvider,
    IHubContext<DisruptionHub> hubContext,
    ILogger<DisruptionSimulationWorker> logger) : BackgroundService
{
    private readonly string[] _disruptionTypes = ["Typhoon", "Labor Strike", "Customs Delay", "Power Outage", "Material Shortage"];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Disruption Simulation Worker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait anywhere from 10 to 30 seconds before triggering the next disruption
            var delay = Random.Shared.Next(10000, 30000);
            await Task.Delay(delay, stoppingToken);

            await SimulateDisruptionAsync(stoppingToken);
        }
    }

    private async Task SimulateDisruptionAsync(CancellationToken stoppingToken)
    {
        // Create a new scope to safely resolve the DbContext
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EcoTrackDbContext>();

        // Find a random supply node to disrupt
        var nodes = await dbContext.SupplyNodes.ToListAsync(stoppingToken);
        if (nodes.Count == 0) return;

        var targetNode = nodes[Random.Shared.Next(nodes.Count)];

        // Generate the disruption
        var disruption = new Disruption
        {
            SupplyNodeId = targetNode.Id,
            Type = _disruptionTypes[Random.Shared.Next(_disruptionTypes.Length)],
            Description = $"Unexpected event impacting operations at node {targetNode.Id}",
            SeverityLevel = Random.Shared.Next(1, 11) // 1 to 10 scale
        };

        // Save to database
        dbContext.Disruptions.Add(disruption);
        await dbContext.SaveChangesAsync(stoppingToken);

        logger.LogWarning("New Disruption: {Type} at Node {NodeId} (Severity: {Severity})",
            disruption.Type, targetNode.Id, disruption.SeverityLevel);

        // ----------------------------------------------------
        // THE FIX: Map the Entity to the flat DTO to prevent JSON Object Cycles
        // ----------------------------------------------------
        var payload = new EcoTrack.Application.DTOs.DisruptionDto(
            disruption.Id,
            disruption.SupplyNodeId,
            disruption.Type,
            disruption.Description,
            disruption.SeverityLevel,
            disruption.IsActive,
            disruption.ReportedAt
        );

        // Broadcast the flat payload to all connected Angular clients
        await hubContext.Clients.All.SendAsync("ReceiveDisruption", payload, stoppingToken);
    }
}