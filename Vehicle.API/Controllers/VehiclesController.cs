using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Vehicles.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class VehiclesController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public VehiclesController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [HttpGet]
        public async Task<QueryResultList<VechicleList>> ListAsync([FromQuery] ListVehicles query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [AllowAnonymous]
        [HttpGet("select")]
        public async Task<QueryResultList<VehicleSelectList>> ListSelectAsync([FromQuery] ListVehiclesSelect query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [AllowAnonymous]
        [HttpGet("colors")]
        public async Task<QueryResultList<ColorList>> ListColorsAsync([FromQuery] ListColors query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [AllowAnonymous]
        [HttpGet("fuels")]
        public async Task<QueryResultList<FuelList>> ListFuelsAsync([FromQuery] ListFuels query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpGet("{id}")]
        public async Task<QueryResultOne<VehicleDetail>> GetAsync(string id, [FromQuery] GetVehicle query)
        {
            query.Id = id;
            return GetResult(await _queriesHanlder.Handle(query));
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/photo")]
        public async Task<ActionResult> GetPhotoAsync(string id, [FromQuery] GetVehiclePhoto query)
        {
            query.Id = id;
            var result = await _queriesHanlder.Handle(query);
            if (result.Status != EStatusCode.Success) return NotFound(result);
            return File(result.Data.Bytes, "image/jpeg", result.Data.FileName);
        }

        [HttpPost]
        public async Task<MutationResult> CreateAsync([FromBody] CreateVehicle mutation)
        {
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpPut("{id}")]
        public async Task<MutationResult> UpdateAsync(string id, [FromBody] UpdateVehicle mutation) { 
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpPatch("{id}/sell")]
        public async Task<MutationResult> SellAsync(string id, [FromBody] SellVehicle mutation)
        {
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpDelete("{id}")]
        public async Task<MutationResult> DeleteAsync(string id) { 
            var mutation = new DeleteVehicle { Id = id };
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
