using Dapper;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class ListVehiclesSelect : IQueryList<VehicleSelectList>
    {
        public async Task<QueryResultList<VehicleSelectList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<VehicleSelectList>>(null);
        }

        public async Task<QueryResultList<VehicleSelectList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select v.id, v.name 
                from view_vehicles_list v
                order by v.name asc;
            ";
            var vehicles = await handler.DbConnection.QueryAsync<VehicleSelectList>(sql);
            return new QueryResultList<VehicleSelectList>(vehicles);
        }
    }
}
