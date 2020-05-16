using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Models.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Queries.Models;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class ModelsController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public ModelsController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<QueryResultList<ModelList>>> ListAsync([FromQuery] ListModels query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QueryResultOne<ModelDetail>>> GetAsync(string id, [FromQuery] GetModel query) { 
            query.Id = id;
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpGet("select")]
        public async Task<ActionResult<QueryResultList<ModelSelectList>>> GetSelectListAsync([FromQuery] ListModelsSelect query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpPost]
        public async Task<ActionResult<MutationResult>> CreateAsync([FromBody] CreateModel mutation) { 
            return GetResult(await _mutationsHanlder.Handle(mutation));    
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MutationResult>> UpdateAsync(string id, [FromBody] UpdateModel mutation)
        {
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MutationResult>> DeleteAsync(string id) { 
            var mutation = new DeleteModel{ Id = id };
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
