using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Mutations.Vehicles.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            yield return new object[] { EStatusCode.Success,     new CreateVehicle { Id = RandomId.NewId(), FuelId = EFuel.Flex, ColorId = EColor.Black, BrandId = RandomId.NewId(), ModelId = RandomId.NewId() } };
        }   
        
        [Theory]
        [MemberData(nameof(CreateVehicleData))]
        public async void CreateVehicle(
            EStatusCode expectedStatus, 
            CreateVehicle mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound) { 
                EntitiesFactory.NewModel(id: mutation.ModelId).Save(); 
                EntitiesFactory.NewBrand(id: mutation.BrandId).Save();
            }

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
                Assert.Equal(mutation.BrandId, vehicleDb.Brand.Id);
            }
        }
    }
}
