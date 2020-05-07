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
    public class UpdateModel : IMutation
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 200) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Name)} is invalid");
            var exists = await handler.DbContext.Models.AnyAsync(m => m.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Model with {nameof(Id)} does not exists");
            var existsName = await handler.DbContext.Models.AnyAsync(m => m.Id != Id && m.Name == Name);
            if (existsName) return new MutationResult(EStatusCode.Conflict, $"Model with {nameof(Name)} already exists");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var model = await handler.DbContext.Models.FindAsync(Id);
            model.Name = Name;
            handler.DbContext.Models.Update(model);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
