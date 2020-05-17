using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Questor.Vehicle.Domain.Mutations.Reservations.Mutations
{
    public class FinishReservation : IMutation
    {
        public string Id { get; set; }
        public DateTimeOffset? DateSale { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is require");
            if (DateSale == null) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(DateSale)} require");
            var exists = await handler.DbContext.Reservations.AnyAsync(a => a.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Reservation with id: {Id} does not exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var reservation = await handler.DbContext.Reservations
                .Include(i => i.Announcement)
                .Where(r => r.Id == Id)
                .FirstOrDefaultAsync();
            reservation.SetDateSale(DateSale.Value.Date);
            reservation.Announcement.SetDateSale(DateSale.Value.Date);
            handler.DbContext.Update(reservation);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
