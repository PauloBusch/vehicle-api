using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Announcements.Mutations
{
    public class UpdateAnnouncement : IMutation
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
            var exists = await handler.DbContext.Announcements.AnyAsync(a => a.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Announcement with id: {Id} does not exists");
            var existsVehicle = await handler.DbContext.Vehicles.AnyAsync(v => v.Id == VehicleId);
            if (!existsVehicle) return new MutationResult(EStatusCode.NotFound, $"Vehicle with id: {VehicleId} does not exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var announcement = await handler.DbContext.Announcements.FindAsync(Id);
            announcement.SetData(
               pricePurchase: PricePurchase,
               priceSale: PriceSale,
               dateSale: announcement.DateSale,
               vehicleId: VehicleId
            );
            handler.DbContext.Update(announcement);
            var rows = await handler.DbContext.SaveChangesAsync(); 
            return new MutationResult(rows);
        }
    }
}
