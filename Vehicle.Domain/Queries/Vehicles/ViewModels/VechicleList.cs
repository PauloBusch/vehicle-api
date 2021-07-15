using Questor.Vehicle.Domain.Utils.Interfaces;

namespace Questor.Vehicle.Domain.Queries.Vehicles.ViewModels
{
    public class VechicleList : IViewModel
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public string Board { get; set; }
        public string ColorName { get; set; }
        public string ColorHex { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string FuelName { get; set; }
    }
}
