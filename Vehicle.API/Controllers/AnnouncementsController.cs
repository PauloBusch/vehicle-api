using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Announcements.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Queries.Announcements;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class AnnouncementsController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public AnnouncementsController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<QueryResultList<AnnouncementList>>> ListAsync([FromQuery] ListAnnouncement query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpGet("select")]
        [AllowAnonymous]
        public async Task<ActionResult<QueryResultList<AnnouncementSelectList>>> ListSelectAsync([FromQuery] ListAnnouncementSelect query)
        {
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QueryResultOne<AnnouncementDetail>>> GetAsync(string id, [FromQuery] GetAnnouncement query) { 
            query.Id = id;
            return GetResult(await _queriesHanlder.Handle(query));
        }

        [HttpPost]
        public async Task<ActionResult<MutationResult>> CreateAsync([FromBody] CreateAnnouncement mutation) { 
            return GetResult(await _mutationsHanlder.Handle(mutation));    
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MutationResult>> UpdateAsync(string id, [FromBody] UpdateAnnouncement mutation) { 
            mutation.Id = id;
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MutationResult>> DeleteAsync(string id)
        {
            var mutation = new DeleteAnnouncement{ Id = id };
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
