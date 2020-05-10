using Dapper;
using Questor.Vehicle.Domain.Queries.Reservations.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Reservations
{
    public class ListReservations : IQueryList<ReservationList>
    {
        public async Task<QueryResultList<ReservationList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<ReservationList>>(null);
        }

        public async Task<QueryResultList<ReservationList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select 
                    r.id, c.name as contact_name, c.phone as contact_phone,
                    m.name as vehicle_model_name, b.name as vehicle_brand_name
                from reservations r
                    join contacts c on c.id=r.id_contact
                    join announcements a on a.id=r.id_announcement
                    join vehicles v on v.id=a.id_vehicle
                    join models m on m.id=v.id_model
                    join brands b on b.id=v.id_brand
                where r.date_sale is null
                order by r.date_creation asc;
            ";
            var reservations = await handler.DbConnection.QueryAsync<ReservationList>(sql);
            return new QueryResultList<ReservationList>(reservations);
        }
    }
}
