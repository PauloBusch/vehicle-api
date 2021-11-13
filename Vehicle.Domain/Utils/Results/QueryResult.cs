using Newtonsoft.Json;
using Questor.Vehicle.Domain.Utils.Enums;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public abstract class QueryResult {
        [JsonIgnore] public EStatusCode Status { get; protected set; }
        public string Message { get; protected set; }

        protected QueryResult() { }

        public QueryResult(EStatusCode status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
