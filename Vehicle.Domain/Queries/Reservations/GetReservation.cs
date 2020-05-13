using Questor.Vehicle.Domain.Queries.Reservations.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Reservations
{
    public class GetReservation : IQueryOne<ReservationDetail>
    {
        public string Id { get; set; }

        public Task<QueryResultOne<ReservationDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResultOne<ReservationDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
