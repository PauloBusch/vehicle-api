using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Vehicles
{
    public class ListVehiclesTest : BaseTests
    {
        public ListVehiclesTest(VehicleFixture fixture, string url) : base(fixture, url) { }
    
        public static IEnumerable<object[]> ListVehiclesData()
        {
            yield return new object[] {  };
        }

        [Theory]
        [MemberData(nameof(ListVehiclesData))]
        public async void ListVehicles(
            EStatusCode expectedStatus,
            ListVehicles query
        ) {
            var vehicle = EntitiesFactory.NewVehicle().Save();
            var (status, result) = await Request.Get<QueryResultList<VechicleList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotNull(result.Data);
                Assert.Contains(result.Data, v => v.Id == vehicle.Id);
            }
        }
    }
}
