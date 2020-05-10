using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Vehicles
{
    public class ListVehiclesTest : BaseTests
    {
        public ListVehiclesTest(VehicleFixture fixture) : base(fixture, "/vehicles") { }
    
        public static IEnumerable<object[]> ListVehiclesData()
        {
            yield return new object[] { EStatusCode.Success, new ListVehicles { } };
            yield return new object[] { EStatusCode.Success, new ListVehicles { Year = 2010, BrandId = RandomId.NewId(), ModelId = RandomId.NewId(), FuelId = EFuel.Alcohol, ColorId = EColor.Black } };
        }

        [Theory]
        [MemberData(nameof(ListVehiclesData))]
        public async void ListVehicles(
            EStatusCode expectedStatus,
            ListVehicles query
        ) {
            var vehicle = EntitiesFactory.NewVehicle(
                brandId: query.BrandId, 
                modelId: query.ModelId,
                fuel: query.FuelId,
                color: query.ColorId
            ).Save();
            var (status, result) = await Request.Get<QueryResultListTest<VechicleList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotNull(result.Data);
                var vehicleResult = result.Data.FirstOrDefault(v => v.Id == vehicle.Id);
                Assert.NotNull(vehicleResult);
                Assert.Equal(vehicle.Year, vehicleResult.Year);
                Assert.Equal(vehicle.Brand.Name, vehicleResult.BrandName);
                Assert.Equal(vehicle.Model.Name, vehicleResult.ModelName);
                Assert.NotNull(vehicleResult.ColorHex);
                Assert.NotNull(vehicleResult.ColorName);
                Assert.NotNull(vehicleResult.FuelName);
            }
        }
    }
}
