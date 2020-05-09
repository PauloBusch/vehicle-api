using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Vehicles.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class VehicleController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public VehicleController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [HttpGet]
        public async Task<ActionResult<QueryResultList<VechicleList>>> ListAsync([FromQuery] ListVehicles query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpPost]
        public async Task<ActionResult<MutationResult>> CreateAsync([FromBody] CreateVehicle mutation)
        {
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MutationResult>> UpdateAsync(string id, [FromBody] UpdateVehicle mutation) { 
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MutationResult>> DeleteAsync(string id) { 
            var mutation = new DeleteVehicle { Id = id };
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
