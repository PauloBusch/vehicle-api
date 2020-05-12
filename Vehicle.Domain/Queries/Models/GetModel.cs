using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Models
{
    public class GetModel : IQueryOne<ModelDetail>
    {
        public string Id { get; set; }

        public Task<QueryResultOne<ModelDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResultOne<ModelDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
