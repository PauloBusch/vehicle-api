using Dapper;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Announcements
{
    public class ListAnnouncementSelect : IQueryList<AnnouncementSelectList>
    {
        public bool IncludeReserved { get; set; }
        public async Task<QueryResultList<AnnouncementSelectList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            return await Task.FromResult<QueryResultList<AnnouncementSelectList>>(null);
        }

        public async Task<QueryResultList<AnnouncementSelectList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            var sql = $@"
                select a.id, a.vehicle_name as name
                from view_announcements_list a
                where a.date_sale is null
                {(!IncludeReserved ? " and not exists(select r.id from reservations r where r.id_announcement=a.id)" : null)}
                order by a.vehicle_name asc;
            ";
            var announcements = await handler.DbConnection.QueryAsync<AnnouncementSelectList>(sql);
            return new QueryResultList<AnnouncementSelectList>(announcements);
        }
    }
}
