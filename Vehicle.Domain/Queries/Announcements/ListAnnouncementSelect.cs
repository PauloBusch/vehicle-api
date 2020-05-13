using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Queries.Announcements
{
    public class ListAnnouncementSelect : IQueryList<AnnouncementSelectList>
    {
        public Task<QueryResultList<AnnouncementSelectList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResultList<AnnouncementSelectList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
