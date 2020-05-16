using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Models.ViewModels
{
    public class ModelDetail : IViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
