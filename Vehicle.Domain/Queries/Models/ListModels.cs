using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Questor.Vehicle.Domain.Utils.Results;
using Dapper;

namespace Questor.Vehicle.Domain.Queries.Models
{
    public class ListModels : IQueryList<ModelList>
    {
        public async Task<QueryResultList<ModelList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<ModelList>>(null);
        }

        public async Task<QueryResultList<ModelList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select id, name
                from models
                order by name asc;
            ";
            var models = await handler.DbConnection.QueryAsync<ModelList>(sql);
            return new QueryResultList<ModelList>(models);
        }
    }
}
