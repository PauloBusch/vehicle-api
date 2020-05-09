using Newtonsoft.Json;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public abstract class QueryResult {
        [JsonIgnore] public EStatusCode Status { get; set; }
        public string Message { get; set; }

        protected QueryResult() {}
        public QueryResult(EStatusCode status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
