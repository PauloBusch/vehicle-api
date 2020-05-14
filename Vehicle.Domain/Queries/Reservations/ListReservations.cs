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
                    r.id, r.contact_name, r.contact_phone,
                    r.vehicle_model_name, r.vehicle_brand_name
                from view_reservations_list r
                where r.date_sale is null
                order by r.date_creation asc;
            ";
            var reservations = await handler.DbConnection.QueryAsync<ReservationList>(sql);
            return new QueryResultList<ReservationList>(reservations);
        }
    }
}
