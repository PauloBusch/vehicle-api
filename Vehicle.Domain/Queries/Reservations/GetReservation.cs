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
                    r.id, c.name as contact_name, c.phone as contact_phone,
                    a.id as announcement_id,
                    concat(b.name, ' - ', m.name) as announcement_name
                from reservations r 
                    join contacts c on c.id=r.id_contact
                    join announcements a on a.id=r.id_announcement
                    join vehicles v on v.id=a.id_vehicle
                    join models m on m.id=v.id_model
                    join brands b on b.id=v.id_brand
                where r.id=@Id
            ";
            var reservation = await handler.DbConnection.QueryFirstOrDefaultAsync<ReservationDetail>(sql, new { Id });
            if (reservation == null) return new QueryResultOne<ReservationDetail>(EStatusCode.NotFound, $"Reservation with id: {Id} does not exists");
            return new QueryResultOne<ReservationDetail>(reservation);
        }
    }
}
