using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Vehicles.ViewModels
{
    public class VehicleDetail : IViewModel
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public DateTime? PhotoDate { get; set; }
        public string ImageBase64 { get; set; }
        public EColor ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public EFuel FuelId { get; set; }
        public string FuelName { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string ModelId { get; set; }
        public string ModelName { get; set; }
    }
}
