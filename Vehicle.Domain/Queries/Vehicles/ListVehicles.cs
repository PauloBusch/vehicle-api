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
                    v.id, v.year, c.name as color_name, c.hex as color_hex,
                    b.name as brand_name, m.name as model_name, f.name as fuel_name
                from vehicles v
                    join fuels f on f.id=v.id_fuel
                    join colors c on c.id=v.id_color
                    join brands b on b.id=v.id_brand
                    join models m on m.id=v.id_model
                where 1=1
                    {(Year    != null ? "and v.year=@Year "        : null)}
                    {(FuelId  != null ? "and v.id_fuel=@FuelId "   : null)}
                    {(ColorId != null ? "and v.id_color=@ColorId " : null)}
                    {(ModelId != null ? "and v.id_model=@ModelId " : null)}
                    {(BrandId != null ? "and v.id_brand=@BrandId " : null)}
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
