using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public class QueryResultOne<T> : QueryResult
        where T : IViewModel
    {
        public T Data { get; private set; }

        protected QueryResultOne() { }

        public QueryResultOne(T data)
        {
            Data = data;
            Status = EStatusCode.Success;
        }

        public QueryResultOne(EStatusCode status, string message) 
            : base(status, message) { }
    }
}
