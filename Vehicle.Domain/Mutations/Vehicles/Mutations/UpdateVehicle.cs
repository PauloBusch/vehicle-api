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
    public class UpdateVehicle : IMutation
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public int? Amount { get; set; }
        public EColor ColorId { get; set; }
        public EFuel FuelId { get; set; }
        public string ModelId { get; set; }
        public string ImageBase64 { get; set; }
        
        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            if (Year <= 1950) return new MutationResult(EStatusCode.InvalidData, $"Paramter {nameof(Year)} require upper 150");
            if (!Enum.IsDefined(typeof(EFuel), FuelId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(FuelId)} is required");
            if (!Enum.IsDefined(typeof(EColor), ColorId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(ColorId)} is required");
            if (string.IsNullOrWhiteSpace(ModelId)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(ModelId)} is required");
            var exists = await handler.DbContext.Vehicles.AnyAsync(v => v.Id == Id);
            if (!exists) return new MutationResult(EStatusCode.NotFound, $"Vehicle wtih {nameof(Id)} does not exists");
            var existsModel = await handler.DbContext.Models.AnyAsync(m => m.Id == ModelId);
            if (!existsModel) return new MutationResult(EStatusCode.NotFound, $"Model with id: {ModelId} does not exsits");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var vehicle = await handler.DbContext.Vehicles.FindAsync(Id);
            vehicle.SetData(
                year: Year,
                amount: Amount,
                color: ColorId,
                fuel: FuelId,
                modelId: ModelId,
                photoDate: string.IsNullOrWhiteSpace(ImageBase64)
                    ? null
                    : (Nullable<DateTime>)DateTime.Now
            ); 
            var fileName = $"{vehicle.Id}.jpg";
            if (!string.IsNullOrWhiteSpace(ImageBase64))
                await Base64.SaveAsync(ImageBase64, EPath.Photos, fileName);
            else 
                Base64.Remove(EPath.Photos, fileName);
            handler.DbContext.Update(vehicle);
            var rows = await handler.DbContext.SaveChangesAsync();
            return new MutationResult(rows);
        }
    }
}
