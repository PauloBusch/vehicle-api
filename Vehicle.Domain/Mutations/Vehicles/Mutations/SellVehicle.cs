using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Vehicles.Mutations
{
    public class SellVehicle : IMutation
    {
        public string Id { get; set; }
        public DateTime? DateSale { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            if (DateSale == null) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(DateSale)} is require");
            var existsVehicle = await handler.DbContext.Vehicles.AnyAsync(v => v.Id == Id);
            if (!existsVehicle) return new MutationResult(EStatusCode.NotFound, $"Vehicle with id: {Id} does not exists");
            var existsAnnouncement = await handler.DbContext.Announcements.AnyAsync(a => a.VehicleId == Id && a.DateSale == null);
            if (!existsAnnouncement) return new MutationResult(EStatusCode.NotFound, $"Announcement by VehicleId: {Id} does not exists or already Sell");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var vehicle = await handler.DbContext.Vehicles.FindAsync(Id);
            vehicle.Sell(DateSale.Value);
            handler.DbContext.Update(vehicle);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
