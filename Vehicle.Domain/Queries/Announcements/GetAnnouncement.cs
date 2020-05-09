using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
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

        public Task<QueryResultOne<AnnouncementDetail>> ValidateAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResultOne<AnnouncementDetail>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
