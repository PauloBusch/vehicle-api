using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Vehicles.Mutations
{
    public class DeleteVehicle : IMutation
    {
        public string Id { get; set; }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
