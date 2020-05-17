using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using System.Threading.Tasks;
using Questor.Vehicle.Domain.Utils.Results;
using Dapper;

namespace Questor.Vehicle.Domain.Queries.Models
{
    public class ListModels : IQueryList<ModelList>
    {
        public string BrandId { get; set; }
        public async Task<QueryResultList<ModelList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<ModelList>>(null);
        }

        public async Task<QueryResultList<ModelList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = $@"
                select m.id, m.name, b.name as brand_name
                from models m
                join brands b on b.id=m.id_brand
                where 1=1
                {(!(string.IsNullOrWhiteSpace(BrandId)) ? "and b.id=@BrandId" : null)}
                order by m.name asc;
            ";
            var models = await handler.DbConnection.QueryAsync<ModelList>(sql, new { BrandId });
            return new QueryResultList<ModelList>(models);
        }
    }
}
