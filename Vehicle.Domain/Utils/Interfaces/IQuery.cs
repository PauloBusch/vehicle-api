using Questor.Vehicle.Domain.Queries;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Utils.Interfaces
{
    public interface IQuery<TResult>
        where TResult : QueryResult
    {
        Task<TResult> ValidateAsync(VehicleQueriesHandler handler);
        Task<TResult> ExecuteAsync(VehicleQueriesHandler handler);
    }

    public interface IQueryOne<T> : IQuery<QueryResultOne<T>> where T : IViewModel { }
    public interface IQueryList<T> : IQuery<QueryResultList<T>> where T : IViewModel { }
}
