using Dapper;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Models
{
    public class ListModelsSelect : IQueryList<ModelSelectList>
    {
        public string BrandId { get; set; }

        public async Task<QueryResultList<ModelSelectList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<ModelSelectList>>(null);
        }

        public async Task<QueryResultList<ModelSelectList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = $@"
                select m.id, m.name
                from models m
                where 1=1
                {(!string.IsNullOrWhiteSpace(BrandId) ? "and m.id_brand=@BrandId" : null)}
                order by m.name asc;
            ";
            var models = await handler.DbConnection.QueryAsync<ModelSelectList>(sql, new { BrandId });
            return new QueryResultList<ModelSelectList>(models);
        }
    }
}
