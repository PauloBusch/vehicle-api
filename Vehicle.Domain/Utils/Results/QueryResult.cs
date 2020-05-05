using Newtonsoft.Json;
using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public class QueryResult<TData>
        where TData : class
    {
        [JsonIgnore] public EStatusCode Status { get; private set; }
        public string Message { get; private set; }
        public TData Data { get; private set; }
        public int TotalRows { get; private set; }

        public QueryResult(TData data, int? totalRows = null)
        {
            this.Data = data;
            this.TotalRows = totalRows ?? 1;
            this.Status = EStatusCode.Success;
        }

        public QueryResult(EStatusCode status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
