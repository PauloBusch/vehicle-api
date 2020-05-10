using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;

namespace Questor.Vehicle.API.Controllers
{
    [ApiController]
    [Authorize("RequireAuthentication")]
    [Route("api/vehicle/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult<MutationResult> GetResult(MutationResult result) => GetResult(result, result.Status);
        protected ActionResult<QueryResultOne<T>> GetResult<T>(QueryResultOne<T> result) where T : IViewModel => GetResult(result, result.Status);
        protected ActionResult<QueryResultList<T>> GetResult<T>(QueryResultList<T> result) where T : IViewModel => GetResult(result, result.Status);
        
        private ActionResult<TResult> GetResult<TResult>(TResult result, EStatusCode statusCode) {
            Response.StatusCode = (int)statusCode;
            return result;
        }
    }
}
