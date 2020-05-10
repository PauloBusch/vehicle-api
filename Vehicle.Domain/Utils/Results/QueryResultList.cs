using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public class QueryResultList<T> : QueryResult
        where T : IViewModel
    {
        public T[] Data { get; private set; }
        public Int64 TotalRows { get; private set; }

        public QueryResultList(IEnumerable<T> data, Int64? totalRows = null)
        {
            Data = data.ToArray();
            TotalRows = totalRows ?? data?.Count() ?? 1;
            Status = EStatusCode.Success;
        }

        public QueryResultList(EStatusCode status, string message)
            : base(status, message) { }
    }
}
