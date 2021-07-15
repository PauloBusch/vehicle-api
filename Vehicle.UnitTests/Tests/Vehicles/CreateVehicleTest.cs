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
    public class CreateVehicleTest : TestsBase
    {
        public CreateVehicleTest(VehicleFixture fixture) : base(fixture) { }
    
        public static IEnumerable<object[]> CreateVehicleData()
        {
            yield return new object[] { EStatusCode.InvalidData, new CreateVehicle { } };
            yield return new object[] { EStatusCode.InvalidData, new CreateVehicle { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new CreateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex } };
            yield return new object[] { EStatusCode.InvalidData, new CreateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black } };
            yield return new object[] { EStatusCode.InvalidData, new CreateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010 } };
            yield return new object[] { EStatusCode.NotFound,    new CreateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010, ModelId = RandomId.NewId(), Board = RandomId.NewId() }, false };
            yield return new object[] { EStatusCode.Success,     new CreateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, Year = 2010, ModelId = RandomId.NewId(), Board = RandomId.NewId() }, true };
        }   
        
        [Theory]
        [MemberData(nameof(CreateVehicleData))]
        public async void CreateVehicle(
            EStatusCode expectedStatus, 
            CreateVehicle mutation,
            bool? withModel = false
        ) {
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
                Assert.Equal(mutation.Board, vehicleDb.Board);
                Assert.Equal(mutation.ColorId, vehicleDb.Color);
                Assert.Equal(mutation.FuelId, vehicleDb.Fuel);
                Assert.Equal(mutation.ModelId, vehicleDb.Model.Id);
            }
        }
    }
}
