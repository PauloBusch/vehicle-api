using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Announcements.ViewModels
{
    public class AnnouncementList : IViewModel
    {
        public string Id { get; set; }
        public DateTime? DateSale { get; set; }
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public string VehicleName { get; set; }
    }
}
