using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Queries.Models;
using Questor.Vehicle.Domain.Utils.Results;
using Questor.Vehicle.Domain.Utils.Enums;
using Xunit;
using System.Collections.Generic;
using Vehicle.IntegrationTests.Utils.Results;

namespace Vehicle.IntegrationTests.Tests.Models
{
    public class ListModelsTest : BaseTests
    {
        public ListModelsTest(VehicleFixture fixture) : base(fixture, "/models") { }

        public static IEnumerable<object[]> ListModelsData()
        {
            yield return new object[] { EStatusCode.Success, new ListModels { } };
        }

        [Theory]
        [MemberData(nameof(ListModelsData))]
        public async void ListModels(
            EStatusCode expectedStatus,
            ListModels query
        ) {
            var model = EntitiesFactory.NewModel().Save();
            var (status, result) = await Request.Get<QueryResultListTest<ModelList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotEmpty(result.Data);
                Assert.Contains(result.Data, d => d.Id == model.Id);
            }
        }
    }
}
