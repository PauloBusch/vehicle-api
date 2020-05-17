using Dapper;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Files;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class GetVehicle : IQueryOne<VehicleDetail>
    {
        public string Id { get; set; }

        public async Task<QueryResultOne<VehicleDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new QueryResultOne<VehicleDetail>(EStatusCode.InvalidData, $"Paramter {nameof(Id)} is required");
            return await Task.FromResult<QueryResultOne<VehicleDetail>>(null); 
        }

        public async Task<QueryResultOne<VehicleDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select 
                    v.id, v.year, v.color_id, v.color_name, v.color_hex, photo_date,
                    v.fuel_id, v.fuel_name, v.brand_id, v.brand_name, v.model_id, v.model_name
                from view_vehicles_list v
                where v.id=@Id
            ";
            var vehicle = await handler.DbConnection.QueryFirstOrDefaultAsync<VehicleDetail>(sql, new { Id });
            if (vehicle == null) return new QueryResultOne<VehicleDetail>(EStatusCode.NotFound, $"Vehicle with {nameof(Id)} does not exists");
            if (vehicle.PhotoDate != null) vehicle.ImageBase64 = await Base64.LoadBase64Async(EPath.Photos, $"{vehicle.Id}.jpg");
            return new QueryResultOne<VehicleDetail>(vehicle);
        }
    }
}
