using Dapper;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
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

        public async Task<QueryResultOne<ModelDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new QueryResultOne<ModelDetail>(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            return await Task.FromResult<QueryResultOne<ModelDetail>>(null);
        }

        public async Task<QueryResultOne<ModelDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select m.id, m.name, b.id as brand_id, b.name as brand_name
                from models m
                join brands b on b.id=m.id_brand
                where m.id=@Id;
            ";
            var model = await handler.DbConnection.QueryFirstOrDefaultAsync<ModelDetail>(sql, new { Id });
            if (model == null) return new QueryResultOne<ModelDetail>(EStatusCode.NotFound, $"Model with id: {Id} is not found");
            return new QueryResultOne<ModelDetail>(model);
        }
    }
}
