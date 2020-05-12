using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Brands.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Queries.Brands;
using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public BrandsController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [HttpGet]
        public async Task<ActionResult<QueryResultList<BrandList>>> ListAsync([FromQuery] ListBrands query) {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpPost]
        public async Task<ActionResult<MutationResult>> CreateAsync([FromBody] CreateBrand mutation)
        {
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MutationResult>> CreateAsync(string id, [FromBody] UpdateBrand mutation)
        {
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MutationResult>> CreateAsync(string id)
        {
            var mutation = new DeleteBrand { Id = id };
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
