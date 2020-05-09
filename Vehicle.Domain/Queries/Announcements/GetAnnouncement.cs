using Dapper;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Announcements
{
    public class GetAnnouncement : IQueryOne<AnnouncementDetail>
    {
        public string Id { get; set; }

        public async Task<QueryResultOne<AnnouncementDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id)) return new QueryResultOne<AnnouncementDetail>(EStatusCode.InvalidData, $"Parameter {nameof(Id)} is require");
            return await Task.FromResult<QueryResultOne<AnnouncementDetail>>(null);
        }

        public async Task<QueryResultOne<AnnouncementDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = @"
                select
                    a.id, a.price_purchase, a.price_sale, a.date_sale,
                    v.id as vehicle_id, v.year as vehicle_year,
                    m.name as vehicle_model, b.name as vehicle_brand
                from announcements a
                    join vehicles v on v.id=a.id_vehicle
                    join models m on m.id=v.id_model
                    join brands b on b.id=v.id_brand
                where a.id=@Id
            ";
            var announcement = await handler.DbConnection.QueryFirstOrDefaultAsync<AnnouncementDetail>(sql, new { Id });
            if (announcement == null) return new QueryResultOne<AnnouncementDetail>(EStatusCode.NotFound, $"Announcement with id: {Id} does not exists");
            return new QueryResultOne<AnnouncementDetail>(announcement);
        }
    }
}
