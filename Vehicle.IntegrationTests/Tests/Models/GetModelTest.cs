using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Queries.Models;
using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Models
{
    public class GetModelTest : BaseTests
    {
        public GetModelTest(VehicleFixture fixture) : base(fixture, "/models") { }

        public static IEnumerable<object[]> GetModelData() { 
            yield return new object[] { EStatusCode.InvalidData, new GetModel { } };
            yield return new object[] { EStatusCode.NotFound, new GetModel { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new GetModel { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(GetModelData))]
        public async void GetModel(
            EStatusCode expectedStatus,
            GetModel query
        ) {
            var model = null as Model;
            if (expectedStatus != EStatusCode.NotFound)
                model = EntitiesFactory.NewModel(id: query.Id).Save();
            var (status, result) = await Request.Get<QueryResultOneTest<ModelDetail>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var modelResult = result.Data;
                Assert.NotNull(modelResult);
                Assert.Equal(model.Id, modelResult.Id);
                Assert.Equal(model.Name, modelResult.Name);
            }
        }
    }
}
