using Dapper;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class ListVehicles : IQueryList<VechicleList>
    {
        public int? Year { get; set; }
        public EColor? ColorId { get; set; }
        public EFuel? FuelId { get; set; }
        public string ModelId { get; set; }
        public string BrandId { get; set; }

        public async Task<QueryResultList<VechicleList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<VechicleList>>(null);
        }

        public async Task<QueryResultList<VechicleList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = $@"
                select 
                    v.id, v.year, v.board, v.color_name, v.color_hex,
                    v.brand_name, v.model_name, v.fuel_name
                from view_vehicles_list v
                left join announcements a on a.id_vehicle=v.id
                where a.date_sale is null
                    {(Year != null    ? "and v.year=@Year " : null)}
                    {(FuelId  != null ? "and v.fuel_id=@FuelId "   : null)}
                    {(ColorId != null ? "and v.color_id=@ColorId " : null)}
                    {(!string.IsNullOrWhiteSpace(ModelId) ? "and v.model_id=@ModelId " : null)}
                    {(!string.IsNullOrWhiteSpace(BrandId) ? "and v.brand_id=@BrandId " : null)}
                order by v.date_creation desc;
            ";
            var parameters = new
            {
                Year,
                ColorId,
                FuelId,
                ModelId,
                BrandId
            };
            var vehicles = await handler.DbConnection.QueryAsync<VechicleList>(sql, parameters);
            return new QueryResultList<VechicleList>(vehicles);
        }
    }
}
