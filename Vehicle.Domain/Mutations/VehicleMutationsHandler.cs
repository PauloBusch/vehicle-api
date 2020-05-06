using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations
{
    public class VehicleMutationsHandler
    {
        public readonly VehicleMutationsDbContext DbContext;

        public VehicleMutationsHandler(VehicleMutationsDbContext dbContext) { 
            DbContext = dbContext;
        }

        public async Task<MutationResult> Handle(IMutation mutation)
        {
            var validResult = await mutation.ValidateAsync(this);
            if (validResult != null && validResult.Status != EStatusCode.Success) return validResult;
            return await mutation.ExecuteAsync(this);
        }
    }
}
