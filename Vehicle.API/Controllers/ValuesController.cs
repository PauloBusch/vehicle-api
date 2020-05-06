using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace Questor.Vehicle.API.Controllers
{
    public class ValuesController : BaseController
    {
        private readonly VehicleMutationsHandler _mutationsHanlder;
        private readonly VehicleQueriesHandler _queriesHanlder;

        public ValuesController(VehicleMutationsHandler mutationsHandler, VehicleQueriesHandler queriesHandler)
        {
            _mutationsHanlder = mutationsHandler;
            _queriesHanlder = queriesHandler;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAsync()
        {
            var serverId = await GetPublicIpAsync();
            var clientId = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var mutationsDbResult = await _mutationsHanlder.DbContext.CheckConnectionAsync();
            var mutationsDbName = _mutationsHanlder.DbContext.Database.GetDbConnection().Database;
            var queriesDbResult = await _queriesHanlder.DbContext.CheckConnectionAsync();
            var queriesDbName = _queriesHanlder.DbContext.Database.GetDbConnection().Database;
            
            var message = $"" +
                $"Time Server..: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}\n\n" +
                $"Server Ip....: {serverId}\n" +
                $"Client Ip....: {clientId}\n\n" +
                $"Db Mutations.: {mutationsDbName} - {(mutationsDbResult.canConnect ? "OK" : mutationsDbResult.errorMessage)}\n" +
                $"Db Queries...: {queriesDbName} - {(queriesDbResult.canConnect ? "OK" : queriesDbResult.errorMessage)}\n";
            
            return message;
        }

        private async static Task<string> GetPublicIpAsync()
        {
            try
            {
                using (var wc = new WebClient())
                {
                    string str = await wc.DownloadStringTaskAsync("http://checkip.dyndns.org/");
                    var m = Regex.Match(str, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return m?.Value;
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
