using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Files;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class GetVehiclePhoto : IQueryOne<VehiclePhoto>
    {
        public string Id { get; set; }
        public DateTime? PhotoDate { get; set; }

        public async Task<QueryResultOne<VehiclePhoto>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new QueryResultOne<VehiclePhoto>(EStatusCode.InvalidData, $"Paramter {nameof(Id)} is required");
            return await Task.FromResult<QueryResultOne<VehiclePhoto>>(null);
        }

        public async Task<QueryResultOne<VehiclePhoto>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var fileName = $"{Id}.jpg";
            var fileBytes = await Base64.LoadFileAsync(EPath.Photos, fileName);
            if (fileBytes == null) return new QueryResultOne<VehiclePhoto>(EStatusCode.NotFound, $"File with {nameof(Id)}: {Id} does not exists");
            var fileResult = new VehiclePhoto { Bytes = fileBytes, FileName = fileName };
            return new QueryResultOne<VehiclePhoto>(fileResult);
        }
    }
}
