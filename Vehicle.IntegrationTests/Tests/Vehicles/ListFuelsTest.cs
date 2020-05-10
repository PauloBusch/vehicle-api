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
    public class ListFuelsTest : BaseTests
    {
        public ListFuelsTest(VehicleFixture fixture) : base(fixture, "/vehicles/fuels") { }
    
        public static IEnumerable<object[]> ListFuelsData()
        {
            yield return new object[] { EStatusCode.Success, new ListFuels { } };
        }    

        [Theory]
        [MemberData(nameof(ListFuelsData))]
        public async void ListFuels(
            EStatusCode expectedStatus,
            ListFuels query
        ) {
            var (status, result) = await Request.Get<QueryResultListTest<FuelList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotEmpty(result.Data);
                var expectedTotal = Enum.GetValues(typeof(EFuel)).Length;
                Assert.Equal(expectedTotal, result.TotalRows);
            }
        }
    }
}
