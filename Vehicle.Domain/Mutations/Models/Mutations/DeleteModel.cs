using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Models.Mutations
{
    public class DeleteModel : IMutation
    {
        public string Id { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(Id)} is required");
            var exists = await handler.DbContext.Models.AnyAsync(m => m.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Model with {Id} does not exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var model = await handler.DbContext.Models.FindAsync(Id);
            handler.DbContext.Models.Remove(model);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
