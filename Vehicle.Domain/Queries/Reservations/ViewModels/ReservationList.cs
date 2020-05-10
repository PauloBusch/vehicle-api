using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Reservations.ViewModels
{
    public class ReservationList : IViewModel
    {
        public string Id { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string VehicleModelName { get; set; }
        public string VehicleBrandName { get; set; }
    }
}
