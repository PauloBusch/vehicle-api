using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Vehicles.ViewModels
{
    public class VehiclePhoto : IViewModel
    {
        public string FileName { get; set; }
        public byte[] Bytes { get; set; }
    }
}
