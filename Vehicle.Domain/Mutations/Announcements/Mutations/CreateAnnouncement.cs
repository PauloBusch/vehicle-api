using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Announcements.Entities;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Announcements.Mutations
{
    public class CreateAnnouncement : IMutation
    {
        public string Id { get; set; }
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public string VehicleId { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(Id)} is require");
            if (PricePurchase <= 0) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(PricePurchase)} require positive");
            if (PriceSale <= 0) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(PriceSale)} require positive");
            if (string.IsNullOrWhiteSpace(VehicleId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(VehicleId)} is required");
            var existsVehicle = await handler.DbContext.Vehicles.AnyAsync(v => v.Id == VehicleId);
            if (!existsVehicle) return new MutationResult(EStatusCode.NotFound, $"Vehicle with id: {VehicleId} does not exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var announcement = new Announcement(
                id: Id,
                pricePurchase: PricePurchase,
                priceSale: PriceSale,
                vehicleId: VehicleId,
                dateSale: null
            );
            await handler.DbContext.AddAsync(announcement);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
