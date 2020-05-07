using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;

namespace Questor.Vehicle.API.Controllers
{
    [ApiController]
    [Route("api/vehicle/[controller]")]
    public class BaseController : ControllerBase
    {
        protected ActionResult<MutationResult> GetResult(MutationResult result) => GetResult(result, result.Status);
        protected ActionResult<QueryResult<T>> GetResult<T>(QueryResult<T> result) where T : IViewModel => GetResult(result, result.Status);
        
        private ActionResult<TResult> GetResult<TResult>(TResult result, EStatusCode statusCode) {
            Response.StatusCode = (int)statusCode;
            return result;
        }
    }
}
