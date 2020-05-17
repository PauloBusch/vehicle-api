using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Announcements.ViewModels
{
    public class AnnouncementList : IViewModel
    {
        public string Id { get; set; }
        public DateTimeOffset? DateSale { get; set; }
        public decimal PricePurchase { get; set; }
        public decimal PriceSale { get; set; }
        public string VehicleId { get; set; }
        public DateTime? VehiclePhotoDate { get; set; }
        public int VehicleYear { get; set; }
        public string VehicleColorName { get; set; }
        public string VehicleColorHex { get; set; }
        public string VehicleModelName { get; set; }
        public string VehicleBrandName { get; set; }
        public string VehicleFuelName { get; set; }
    }
}
