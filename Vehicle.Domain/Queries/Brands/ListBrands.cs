using Dapper;
using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Brands
{
    public class ListBrands : IQueryList<BrandList>
    {

        public async Task<QueryResultList<BrandList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<BrandList>>(null);
        }

        public async Task<QueryResultList<BrandList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select id, name
                from brands
                order by name;
            ";
            var brands = await handler.DbConnection.QueryAsync<BrandList>(sql);
            return new QueryResultList<BrandList>(brands);
        }
    }
}
