using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Brands
{
    public class GetBrand : IQueryOne<BrandDetail>
    {
        public string Id { get; set; }

        public Task<QueryResultOne<BrandDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResultOne<BrandDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
