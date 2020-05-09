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
    public class DeleteAnnouncement : IMutation
    {
        public string Id { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is require");
            var exists = await handler.DbContext.Announcements.AnyAsync(a => a.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Announcement with id: {Id} does not exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var announcement = await handler.DbContext.Announcements.FindAsync(Id);
            handler.DbContext.Remove(announcement);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
