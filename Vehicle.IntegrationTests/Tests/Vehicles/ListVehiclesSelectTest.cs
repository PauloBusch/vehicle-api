using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Vehicles
{
    public class ListVehiclesSelectTest : BaseTests
    {
        public ListVehiclesSelectTest(VehicleFixture fixture) : base(fixture, "/vehicles/select") { }
    
        public static IEnumerable<object[]> ListVehiclesSelectData()
        {
            yield return new object[] { EStatusCode.Success, new ListVehiclesSelect { } };
            yield return new object[] { EStatusCode.Success, new ListVehiclesSelect { IncludeAnnouncements = true } };
        }

        [Theory]
        [MemberData(nameof(ListVehiclesSelectData))]
        public async void ListVehiclesSelect(
              EStatusCode expectedStatus,
              ListVehiclesSelect query
        ) { 
            var vehicle = EntitiesFactory.NewVehicle().Save();
            var (status, result) = await Request.Get<QueryResultListTest<VehicleSelectList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if(expectedStatus == EStatusCode.Success) { 
                var expectedName = $"{vehicle.Model.Brand.Name} - {vehicle.Model.Name}";
                var announcementResult = result.Data.FirstOrDefault(a => a.Id == vehicle.Id);
                Assert.NotNull(announcementResult); 
                Assert.Equal(expectedName, announcementResult.Name); 
            }
        }
    }
}
