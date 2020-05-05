using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Utils.Interfaces
{
    public interface IMutation
    {
        Task<MutationResult> Validate(VehicleMutationsHandler handler);
        Task<MutationResult> Execute(VehicleMutationsHandler handler);
    }
}
