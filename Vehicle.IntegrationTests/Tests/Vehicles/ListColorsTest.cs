using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Queries.Vehicles.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Vehicles
{
    public class ListColorsTest : BaseTests
    {
        public ListColorsTest(VehicleFixture fixture) : base(fixture, "/vehicles/colors") { }

        public static IEnumerable<object[]> ListColorsData()
        {
            yield return new object[] { EStatusCode.Success, new ListColors { } };
        }

        [Theory]
        [MemberData(nameof(ListColorsData))]
        public async void ListColors(
            EStatusCode expectedStatus,
            ListColors query
        ) {
            var (status, result) = await Request.Get<QueryResultListTest<ColorList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotEmpty(result.Data);
                var expectedTotal = Enum.GetValues(typeof(EColor)).Length;
                Assert.Equal(expectedTotal, result.TotalRows);
            }
        }
    }
}
