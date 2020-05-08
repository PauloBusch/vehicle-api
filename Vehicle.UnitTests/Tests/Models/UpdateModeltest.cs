using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Mutations.Models.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vehicle.UnitTests.Tests.Models
{
    public class UpdateModeltest : TestsBase
    {
        public UpdateModeltest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> UpdateModelData()
        {
            yield return new object[] { EStatusCode.InvalidData, new UpdateModel { } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateModel { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(250) } };
            yield return new object[] { EStatusCode.NotFound,    new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
            yield return new object[] { EStatusCode.Conflict,    new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
            yield return new object[] { EStatusCode.Success,     new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
        }


        [Theory]
        [MemberData(nameof(UpdateModelData))]
        public async Task UpdateModel(
            EStatusCode expectedStatus,
            UpdateModel mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewModel(id: mutation.Id, name: mutation.Name).Save();
            if (expectedStatus == EStatusCode.Conflict)
                EntitiesFactory.NewModel(name: mutation.Name).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var modelDb = await MutationsDbContext.Models.FindAsync(mutation.Id);
                Assert.Equal(mutation.Name, modelDb.Name);
            }
        }
    }
}
