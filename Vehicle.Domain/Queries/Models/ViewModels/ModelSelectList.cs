using Questor.Vehicle.Domain.Utils.Interfaces;

namespace Questor.Vehicle.Domain.Queries.Models.ViewModels
{
    public class ModelSelectList : IViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
