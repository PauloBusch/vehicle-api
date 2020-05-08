using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public class QueryResultOne<T> : QueryResult
        where T : IViewModel
    {
        public T Data { get; private set; }

        public QueryResultOne(T data)
        {
            Data = data;
            Status = EStatusCode.Success;
        }
    }
}
