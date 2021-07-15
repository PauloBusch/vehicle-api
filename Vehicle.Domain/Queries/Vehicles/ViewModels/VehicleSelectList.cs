using Questor.Vehicle.Domain.Utils.Interfaces;

namespace Questor.Vehicle.Domain.Queries.Vehicles.ViewModels
{
    public class VehicleSelectList : IViewModel
    {
        public string Id { get; set; }
        public string Name { get;set; }
    }
}
