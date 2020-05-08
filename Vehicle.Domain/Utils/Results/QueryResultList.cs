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
        public int TotalRows { get; private set; }

        public QueryResultList(IEnumerable<T> data, int? totalRows = null)
        {
            Data = data.ToArray();
            TotalRows = totalRows ?? 1;
            Status = EStatusCode.Success;
        }
    }
}
