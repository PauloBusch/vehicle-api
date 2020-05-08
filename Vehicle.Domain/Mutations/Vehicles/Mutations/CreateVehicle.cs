using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Vehicles.Mutations
{
    public class CreateVehicle : IMutation
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public EFuel FuelId { get; set; }
        public EColor ColorId { get; set; }
        public string BrandId { get; set; }
        public string ModelId { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            throw new NotImplementedException();
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
