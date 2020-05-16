using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
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
    public class UpdateVehicleTest : TestsBase
    {
        public UpdateVehicleTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> UpdateVehicleData()
        {
            yield return new object[] { EStatusCode.InvalidData, new UpdateVehicle { } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateVehicle { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010 } };
            yield return new object[] { EStatusCode.NotFound,    new UpdateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010, ModelId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.NotFound,    new UpdateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010, ModelId = RandomId.NewId() }, false };
            yield return new object[] { EStatusCode.Success,     new UpdateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010, ModelId = RandomId.NewId() }, true  };
        }

        [Theory]
        [MemberData(nameof(UpdateVehicleData))]
        public async void UpdateVehicle(
            EStatusCode expectedStatus,
            UpdateVehicle mutation,
            bool? withModel = false
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewVehicle(id: mutation.Id).Save();
            if (withModel.Value)
                EntitiesFactory.NewModel(id: mutation.ModelId).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var vehicleDb = await MutationsDbContext.Vehicles
                    .Where(v => v.Id == mutation.Id)
                    .FirstOrDefaultAsync();
                Assert.NotNull(vehicleDb);
                Assert.Equal(mutation.Year, vehicleDb.Year);
                Assert.Equal(mutation.ColorId, vehicleDb.Color);
                Assert.Equal(mutation.FuelId, vehicleDb.Fuel);
                Assert.Equal(mutation.ModelId, vehicleDb.Model.Id);
            }
        }
    }
}
