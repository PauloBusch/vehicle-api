using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Brands.Mutations
{
    public class CreateBrand : IMutation
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(Id)} is required");
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 200) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Name)} is invalid");
            var existsName = await handler.DbContext.Brands.AnyAsync(b => b.Name == Name);
            if (existsName) return new MutationResult(EStatusCode.Conflict, $"Brand with {nameof(Name)} already exists");

            return await Task.FromResult<MutationResult>(null);
        }
        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var brand = new Brand { 
                Id = Id,
                Name = Name
            };
            await handler.DbContext.Brands.AddAsync(brand);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
