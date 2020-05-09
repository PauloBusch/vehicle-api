using Dapper;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
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
                    v.id, v.year, c.id as color_id, c.name as color_name, c.hex as color_hex,
                    f.id as fuel_id, f.name as fuel_name, b.id as brand_id, b.name as brand_name,
                    m.id as model_id, m.name as model_name
                from vehicles v
                    join fuels f on f.id=v.id_fuel
                    join colors c on c.id=v.id_color
                    join brands b on b.id=v.id_brand
                    join models m on m.id=v.id_model
                where v.id=@Id
            ";
            var vehicle = await handler.DbConnection.QueryFirstOrDefaultAsync<VehicleDetail>(sql, new { Id });
            if (vehicle == null) return new QueryResultOne<VehicleDetail>(EStatusCode.NotFound, $"Vehicle with {nameof(Id)} does not exists");
            return new QueryResultOne<VehicleDetail>(vehicle);
        }
    }
}
