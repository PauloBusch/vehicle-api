using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Vehicles
{
    public class GetVehicleTest : BaseTests
    {
        public GetVehicleTest(VehicleFixture fixture) : base(fixture, "/vehicles") { }

        public static IEnumerable<object[]> GetVehicleData()
        {
            yield return new object[] { EStatusCode.NotFound, new GetVehicle { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new GetVehicle { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(GetVehicleData))]
        public async void GetVehicle(
            EStatusCode expectedStatus,
            GetVehicle query
        ) {
            var vehicle = null as Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Vehicle;
            if (expectedStatus != EStatusCode.NotFound)
                vehicle = EntitiesFactory.NewVehicle(id: query.Id).Save();

            var (status, result) = await Request.Get<QueryResultOneTest<VehicleDetail>>(new Uri($"{Uri}/{query.Id}"), query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotNull(result.Data);
                var vehicleResult = result.Data;
                Assert.Equal(vehicle.Year, vehicleResult.Year);
                Assert.Equal(vehicle.Model.BrandId, vehicleResult.BrandId);
                Assert.Equal(vehicle.Model.Brand.Name, vehicleResult.BrandName);
                Assert.Equal(vehicle.ModelId, vehicleResult.ModelId);
                Assert.Equal(vehicle.Model.Name, vehicleResult.ModelName);
                Assert.Equal(vehicle.Fuel, vehicleResult.FuelId);
                Assert.Equal(vehicle.Color, vehicleResult.ColorId);
            }
        }
    }
}
