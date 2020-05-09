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
            throw new NotImplementedException();
        }
    }
}
