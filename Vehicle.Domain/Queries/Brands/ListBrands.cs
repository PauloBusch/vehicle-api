using Dapper;
using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Brands
{
    public class ListBrands : IQuery<Brand>
    {
        public async Task<QueryResult<Brand>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResult<Brand>>(null);
        }

        public async Task<QueryResult<Brand>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select id, name
                from brands
                order by name;
            ";
            var brands = await handler.DbConnection.QueryAsync<Brand>(sql);
            return new QueryResult<Brand>(brands);
        }
    }
}
