using EcoTrack.Application.DTOs;
using EcoTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplyChainController(EcoTrackDbContext context) : ControllerBase
{
    [HttpGet("nodes")]
    public async Task<ActionResult<IEnumerable<SupplyNodeDto>>> GetSupplyNodes()
    {
        var nodes = await context.SupplyNodes
            .Include(n => n.Product)
            .Include(n => n.Supplier)
            .OrderBy(n => n.SequenceOrder)
            .Select(n => new SupplyNodeDto(
                n.Id,
                n.Product!.Name,
                n.Supplier!.Name,
                n.NodeType,
                n.Latitude,
                n.Longitude,
                n.SequenceOrder
            ))
            .ToListAsync();

        return Ok(nodes);
    }
}