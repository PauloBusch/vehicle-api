using Dapper;
using Questor.Vehicle.Domain.Queries.Reservations.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
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

        public async Task<QueryResultOne<ReservationDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new QueryResultOne<ReservationDetail>(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is required");
            return await Task.FromResult<QueryResultOne<ReservationDetail>>(null);
        }

        public async Task<QueryResultOne<ReservationDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select 
                    r.id, r.contact_name, r.contact_phone,
                    r.announcement_id, r.announcement_name
                from view_reservations_list r 
                where r.id=@Id
            ";
            var reservation = await handler.DbConnection.QueryFirstOrDefaultAsync<ReservationDetail>(sql, new { Id });
            if (reservation == null) return new QueryResultOne<ReservationDetail>(EStatusCode.NotFound, $"Reservation with id: {Id} does not exists");
            return new QueryResultOne<ReservationDetail>(reservation);
        }
    }
}
