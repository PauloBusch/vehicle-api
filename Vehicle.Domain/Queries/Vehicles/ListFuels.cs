using Dapper;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Vehicles
{
    public class ListFuels : IQueryList<FuelList>
    {
        public async Task<QueryResultList<FuelList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<FuelList>>(null);
        }

        public async Task<QueryResultList<FuelList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select 
                    f.id, f.name
                from fuels f
                order by f.name asc;
            ";
            var fuels = await handler.DbConnection.QueryAsync<FuelList>(sql);
            return new QueryResultList<FuelList>(fuels);
        }
    }
}
