using Dapper;
using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
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

        public async Task<QueryResultOne<BrandDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if(string.IsNullOrWhiteSpace(Id)) return new QueryResultOne<BrandDetail>(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            return await Task.FromResult<QueryResultOne<BrandDetail>>(null);
        }

        public async Task<QueryResultOne<BrandDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select b.id, b.name 
                from brands b
                where b.id=@Id;
            ";
            var brand = await handler.DbConnection.QueryFirstOrDefaultAsync<BrandDetail>(sql, new { Id });
            if (brand == null) return new QueryResultOne<BrandDetail>(EStatusCode.NotFound, $"Brand with id: {Id} does not exists");
            return new QueryResultOne<BrandDetail>(brand);
        }
    }
}
