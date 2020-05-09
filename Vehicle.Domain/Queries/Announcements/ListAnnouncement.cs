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
    public class ListAnnouncement : IQueryList<AnnouncementList>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string SortColumn { get; set; }
        public EOrder SortAsc { get; set; }
        public int? Year { get; set; }
        public bool OnlyUnsold { get; set; }
        public DateTime? DataSale { get; set; }
        public string BrandId { get; set; }
        public string ModelId { get; set; }

        public Task<QueryResultList<AnnouncementList>> ValidateAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResultList<AnnouncementList>> ExecuteAsync(VehicleQueriesHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
