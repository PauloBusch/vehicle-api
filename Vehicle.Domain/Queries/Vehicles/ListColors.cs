using Dapper;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class ListColors : IQueryList<ColorList>
    {
        public async Task<QueryResultList<ColorList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<ColorList>>(null);
        }

        public async Task<QueryResultList<ColorList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select 
                    c.id, c.name, c.hex as color_hex
                from colors c
                order by c.name asc;
            ";
            var colors = await handler.DbConnection.QueryAsync<ColorList>(sql);
            return new QueryResultList<ColorList>(colors);
        }
    }
}
