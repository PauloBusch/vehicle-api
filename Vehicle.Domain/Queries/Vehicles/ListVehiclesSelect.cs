using Dapper;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class ListVehiclesSelect : IQueryList<VehicleSelectList>
    {
        public bool IncludeAnnouncements { get; set; }
        public string ModelId { get; set; }

        public async Task<QueryResultList<VehicleSelectList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<VehicleSelectList>>(null);
        }

        public async Task<QueryResultList<VehicleSelectList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = $@"
                select v.id, v.name 
                from view_vehicles_list v
                where 1=1
                {(!IncludeAnnouncements ? "and not exists(select a.id from announcements a where a.id_vehicle=v.id)" : null)}
                {(!string.IsNullOrWhiteSpace(ModelId) ? "and v.model_id=@ModelId" : null)}
                order by v.name asc;
            ";
            var vehicles = await handler.DbConnection.QueryAsync<VehicleSelectList>(sql, new { ModelId });
            return new QueryResultList<VehicleSelectList>(vehicles);
        }
    }
}
