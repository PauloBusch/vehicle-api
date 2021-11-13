using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Reservations.Mutations
{
    public class UpdateReservation : IMutation
    {
        public string Id { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string AnnouncementId { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is require");
            if (string.IsNullOrWhiteSpace(ContactName) || ContactName.Length > 150) return new MutationResult(EStatusCode.InvalidData, $"Parameter {ContactName} invalid");
            if (string.IsNullOrWhiteSpace(ContactPhone) || ContactPhone.Length > 15) return new MutationResult(EStatusCode.InvalidData, $"Parameter {ContactPhone} invalid");
            if (string.IsNullOrWhiteSpace(AnnouncementId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(AnnouncementId)} is required");
            var exists = await handler.DbContext.Reservations.AnyAsync(r => r.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Reservation with {nameof(Id)}: {Id} does not exists");
            var contactId = await handler.DbContext.Reservations.Where(r => r.Id == Id).Select(r => r.ContactId).FirstOrDefaultAsync();
            var existsPhone = await handler.DbContext.Contacts.AnyAsync(c => c.Id != contactId && c.Phone == ContactPhone);
            if (existsPhone) return new MutationResult(EStatusCode.Conflict, $"Contact with {nameof(ContactPhone)} already exists");
            var existsAnnouncement = await handler.DbContext.Announcements.AnyAsync(a => a.Id == AnnouncementId);
            if (!existsAnnouncement) return new MutationResult(EStatusCode.NotFound, $"Announcement with id: {AnnouncementId} does not exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var reservation = await handler.DbContext.Reservations
                .Include(i => i.Contact)
                .Where(r => r.Id == Id)
                .FirstOrDefaultAsync();
            
            reservation.SetData(
                announcementId: AnnouncementId,
                contact: reservation.Contact
            );
            reservation.Contact.SetData(
                name: ContactName,
                phone: ContactPhone
            );
            handler.DbContext.Update(reservation);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
