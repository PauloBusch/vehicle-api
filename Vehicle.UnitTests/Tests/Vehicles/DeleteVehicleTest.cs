using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Vehicles.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests.Vehicles
{
    public class DeleteVehicleTest : TestsBase
    {
        public DeleteVehicleTest(VehicleFixture fixture) : base(fixture) { }
    
        public static IEnumerable<object[]> DeleteVehicleData()
        {
            yield return new object[] { EStatusCode.InvalidData, new DeleteVehicle { } };
            yield return new object[] { EStatusCode.NotFound, new DeleteVehicle { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new DeleteVehicle { Id = RandomId.NewId() } };
        }    

        [Theory]
        [MemberData(nameof(DeleteVehicleData))]
        public async void DeleteVehicle(
              EStatusCode expectedStatus,
              DeleteVehicle mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewVehicle().Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var exists = await MutationsDbContext.Vehicles.AnyAsync(v => v.Id == mutation.Id);
                Assert.False(exists);
            }
        }
    }
}
