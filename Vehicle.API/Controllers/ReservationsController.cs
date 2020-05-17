using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Reservations.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Queries.Reservations;
using Questor.Vehicle.Domain.Queries.Reservations.ViewModels;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class ReservationsController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public ReservationsController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [HttpGet]
        public async Task<ActionResult<QueryResultList<ReservationList>>> ListAsync([FromQuery] ListReservations query) { 
            return GetResult(await _queriesHanlder.Handle(query));    
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QueryResultOne<ReservationDetail>>> GetAsync(string id, [FromQuery] GetReservation query)
        {
            query.Id = id;
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<MutationResult>> CreateAsync([FromBody] CreateReservation mutation)
        {
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MutationResult>> PutAsync(string id, [FromBody] UpdateReservation mutation)
        {
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpPatch("{id}/finish")]
        public async Task<ActionResult<MutationResult>> FinshAsync(string id, [FromBody] FinishReservation mutation)
        {
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MutationResult>> DeleteAsync(string id) { 
            var mutation = new DeleteReservation { Id = id };
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
