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
    public class ListModels : IQuery<Model>
    {
        public async Task<QueryResult<Model>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResult<Model>>(null);
        }

        public async Task<QueryResult<Model>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select id, name
                from models
                order by name asc;
            ";
            var models = await handler.DbConnection.QueryAsync<Model>(sql);
            return new QueryResult<Model>(models);
        }
    }
}
