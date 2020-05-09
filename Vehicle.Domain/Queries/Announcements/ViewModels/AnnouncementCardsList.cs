using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Announcements.ViewModels
{
    public class AnnouncementCardList : IViewModel
    {
        public string Id { get; set; }
        public DateTime? DateSale { get; set; }
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public int VehicleYear { get; set; }
        public string VehicleColorName { get; set; }
        public string VehicleColorHex { get; set; }
        public string VehicleFuel { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleBrand { get; set; }
    }
}
