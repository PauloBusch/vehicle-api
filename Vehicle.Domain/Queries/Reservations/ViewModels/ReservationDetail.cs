using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Reservations.ViewModels
{
    public class ReservationDetail : IViewModel
    {
        public string Id { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string AnnouncementId { get; set; }
        public string AnnouncementName { get; set; }
    }
}
