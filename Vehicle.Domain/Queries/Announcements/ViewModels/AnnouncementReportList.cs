using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Announcements.ViewModels
{
    public class AnnouncementReportList : IViewModel
    {
        public string Id { get; set; }
        public DateTime? DateSale { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleBrand { get; set; }
        public decimal? Profit { get; set; }
    }
}
