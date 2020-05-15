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
    public class ListVehiclesSelect : BaseTests
    {
        public ListVehiclesSelect(VehicleFixture fixture) : base(fixture, "/vehicles/select") { }
    
        public static IEnumerable<object[]> ListVehiclesSelectData()
        {
            yield return new object[] { EStatusCode.Success, new ListVehiclesSelect { } };
        }

        [Theory]
        [MemberData(nameof(ListVehiclesSelectData))]
        public async void ListVehiclesSelect(
              EStatusCode expectedStatus,
              ListVehiclesSelect query
        ) { 
            var announcement = EntitiesFactory.NewAnnouncement().Save();
            var (status, result) = await Request.Get<QueryResultListTest<VehicleSelectList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if(expectedStatus == EStatusCode.Success) { 
                var announcementResult = result.Data.FirstOrDefault(a => a.Id == announcement.Id);
                Assert.NotNull(announcementResult); 
                var vehicle = announcement.Vehicle;
                var expectedName = $"{vehicle.Brand.Name} - {vehicle.Model.Name}";
                Assert.Equal(expectedName, announcementResult.Name); 
            }

        }
    }
}
