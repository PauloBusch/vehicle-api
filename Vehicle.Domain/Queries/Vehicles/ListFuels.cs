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

        public Task<QueryResultList<FuelList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
