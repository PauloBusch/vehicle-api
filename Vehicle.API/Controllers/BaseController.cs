using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;

namespace Questor.Vehicle.API.Controllers
{
    [ApiController]
    [Authorize("RequireAuthentication")]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected MutationResult GetResult(MutationResult result) => GetResult(result, result.Status);
        protected QueryResultOne<T> GetResult<T>(QueryResultOne<T> result) where T : IViewModel => GetResult(result, result.Status);
        protected QueryResultList<T> GetResult<T>(QueryResultList<T> result) where T : IViewModel => GetResult(result, result.Status);
        
        private TResult GetResult<TResult>(TResult result, EStatusCode statusCode) {
            Response.StatusCode = (int)statusCode;
            return result;
        }
    }
}
