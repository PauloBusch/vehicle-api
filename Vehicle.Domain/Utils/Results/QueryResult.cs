using Newtonsoft.Json;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public class QueryResult<T>
        where T : IViewModel
    {
        [JsonIgnore] public EStatusCode Status { get; private set; }
        public string Message { get; private set; }
        public dynamic Data { get; private set; }
        public int TotalRows { get; private set; }

        public QueryResult(T row)
        {
            this.Data = row;
            this.TotalRows = 1;
            this.Status = EStatusCode.Success;
        }

        public QueryResult(IEnumerable<T> list, int? totalRows = null)
        {
            this.Data = list;
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
