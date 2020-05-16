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
    public class ListAnnouncementReport : IQueryList<AnnouncementReportList>
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public async Task<QueryResultList<AnnouncementReportList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            if (StartDate > EndDate) return new QueryResultList<AnnouncementReportList>(EStatusCode.InvalidData, $"Paramenter {nameof(StartDate)} cannot be greater than the {nameof(EndDate)}");
            return await Task.FromResult<QueryResultList<AnnouncementReportList>>(null);
        }

        public async Task<QueryResultList<AnnouncementReportList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = $@"
                select 
                    a.id, a.date_sale, a.vehicle_model_name, 
                    a.vehicle_brand_name, a.profit
                from view_announcements_list a
                where a.date_sale is not null
                    {(StartDate != null ? " and a.date_sale>=@StartDate" : null)}
                    {(EndDate != null ? " and a.date_sale<=@EndDate" : null)}
                order by a.date_creation desc;
            ";

            var parameters = new {
                StartDate = StartDate?.ToString("yyyy-MM-dd"),    
                EndDate = EndDate?.ToString("yyyy-MM-dd")
            };
            var announcements = await handler.DbConnection.QueryAsync<AnnouncementReportList>(sql, parameters);
            return new QueryResultList<AnnouncementReportList>(announcements);
        }
    }
}
