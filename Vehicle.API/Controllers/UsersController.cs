using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Mutations.Users.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public UsersController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<MutationResult>> LoginAsync([FromBody] LoginUser mutation)
        {
            return GetResult(await _mutationsHanlder.Handle(mutation));
        }
    }
}
