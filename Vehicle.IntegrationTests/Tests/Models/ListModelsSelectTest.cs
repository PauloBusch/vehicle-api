using Questor.Vehicle.Domain.Queries.Models;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Models
{
    public class ListModelsSelectTest : BaseTests
    {
        public ListModelsSelectTest(VehicleFixture fixture) : base(fixture, "/models/select") { }

        public static object EStatus { get; private set; }

        public static IEnumerable<object[]> ListModelsSelectData()
        {
            yield return new object[] { EStatusCode.Success, new ListModelsSelect { } };
            yield return new object[] { EStatusCode.Success, new ListModelsSelect { BrandId = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(ListModelsSelectData))]
        public async void ListModelsSelect(
            EStatusCode expectedStatus,
            ListModelsSelect query
        ) {
            var model = EntitiesFactory.NewModel(brandId: query.BrandId).Save();
            var (status, result) = await Request.Get<QueryResultListTest<ModelSelectList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var modelResult = result.Data.FirstOrDefault(m => m.Id == model.Id);
                Assert.NotNull(modelResult);
                Assert.Equal(model.Name, modelResult.Name);
            }
        }
    }
}
