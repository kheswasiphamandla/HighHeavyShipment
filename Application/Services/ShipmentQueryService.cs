using HighHeavyShipment.Application.DTOs;
using HighHeavyShipment.Domain;
using HighHeavyShipment.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HighHeavyShipment.Application.Services;

public class ShipmentQueryService : IShipmentQueryService
{
    private readonly ShipmentDbContext _context;

    public ShipmentQueryService(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<ShipmentDto?> GetShipmentByIdAsync(Guid shipmentId)
    {
        var shipment = await _context.Shipments
            .Include(s => s.Quotes)
            .FirstOrDefaultAsync(s => s.Id == shipmentId);

        return shipment == null ? null : MapToDto(shipment);
    }

    public async Task<List<ShipmentDto>> GetAllShipmentsAsync()
    {
        var shipments = await _context.Shipments
            .Include(s => s.Quotes)
            .ToListAsync();

        return shipments.Select(MapToDto).ToList();
    }

    private static ShipmentDto MapToDto(Shipment shipment)
    {
        return new ShipmentDto(
            shipment.Id,
            shipment.Reference,
            shipment.Mode,
            shipment.Origin,
            shipment.Destination,
            shipment.WeightKg,
            shipment.Status,
            shipment.CreatedAt,
            shipment.UpdatedAt,
            shipment.Quotes
                .Select(q => new ShipmentQuoteDto(q.Id, q.Phase, q.Amount, q.Currency, q.CreatedAt))
                .ToList());
    }
}
