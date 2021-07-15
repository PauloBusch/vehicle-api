using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Files;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Vehicles.Mutations
{
    public class CreateVehicle : IMutation
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public string Board { get; set; }
        public EFuel FuelId { get; set; }
        public EColor ColorId { get; set; }
        public string ModelId { get; set; }
        public string ImageBase64 { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            if (Year <= 1950) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(Year)} require upper 150");
            if (string.IsNullOrWhiteSpace(Board)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Board)} is required");
            if (!Enum.IsDefined(typeof(EFuel), FuelId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(FuelId)} is required");
            if (!Enum.IsDefined(typeof(EColor), ColorId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(ColorId)} is required");
            if (string.IsNullOrWhiteSpace(ModelId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(ModelId)} is required");
            var existsModel = await handler.DbContext.Models.AnyAsync(m => m.Id == ModelId);
            if (!existsModel) return new MutationResult(EStatusCode.NotFound, $"Model with id: {ModelId} does not exsits");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var vehicle = new Entities.Vehicle(
                id: Id,
                year: Year,
                board: Board,
                fuel: FuelId,
                color: ColorId,
                modelId: ModelId,
                photoDate: string.IsNullOrWhiteSpace(ImageBase64) 
                    ? null 
                    : (DateTime?)DateTime.Now
            );

            if (!string.IsNullOrWhiteSpace(ImageBase64))
                await Base64.SaveAsync(ImageBase64, EPath.Photos, $"{vehicle.Id}.jpg");
            await handler.DbContext.Vehicles.AddAsync(vehicle);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
