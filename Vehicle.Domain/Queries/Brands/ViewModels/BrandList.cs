using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries.Brands.ViewModels
{
    public class BrandList : IViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
