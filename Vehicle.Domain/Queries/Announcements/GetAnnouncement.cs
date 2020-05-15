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
                    a.id, a.price_purchase, a.price_sale, 
                    a.date_sale, a.vehicle_id, a.vehicle_name 
                from view_announcements_list a
                where a.id=@Id
            ";
            var announcement = await handler.DbConnection.QueryFirstOrDefaultAsync<AnnouncementDetail>(sql, new { Id });
            if (announcement == null) return new QueryResultOne<AnnouncementDetail>(EStatusCode.NotFound, $"Announcement with id: {Id} does not exists");
            return new QueryResultOne<AnnouncementDetail>(announcement);
        }
    }
}
