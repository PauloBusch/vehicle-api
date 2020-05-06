using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries
{
    public class VehicleQueriesHandler
    {
        public readonly VehicleQueriesDbContext DbContext;
        public VehicleQueriesHandler(VehicleQueriesDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<QueryResult<TData>> Handle<TData>(IQuery<TData> query) where TData : class
        {
            var validResult = await query.ValidateAsync(this);
            if (validResult != null && validResult.Status != EStatusCode.Success) return validResult;
            return await query.ExecuteAsync(this);
        }
    }
}
