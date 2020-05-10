using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Utils.Metadata;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Questor.Vehicle.API.Authentication
{
    // font: https://jasonwatmore.com/post/2019/10/21/aspnet-core-3-basic-authentication-tutorial-with-example-api
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler
        ) : base(options, logger, encoder, clock)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.Fail("Missing Authorization Header");

            var tokenStr = Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(tokenStr)) return AuthenticateResult.Fail("Missing Authorization Header");

            var isValid = TokenData.Validate(tokenStr);
            if (!isValid) return AuthenticateResult.Fail("Invalid Authorization Header");

            var tokenData = TokenData.Decode(tokenStr);
            if (tokenData == null) return AuthenticateResult.Fail("Invalid Authorization Header");
            _mutationsHanlder.RequestData = tokenData;
            _queriesHanlder.RequestData = tokenData;

            var identity = new ClaimsIdentity(tokenData.GetClaims(), Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
