using Microsoft.EntityFrameworkCore;
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
    public class DeleteModelTest : TestsBase
    {
        public DeleteModelTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> DeleteModelData()
        {
            yield return new object[] { EStatusCode.InvalidData, new DeleteModel { } };
            yield return new object[] { EStatusCode.NotFound,    new DeleteModel { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success,     new DeleteModel { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(DeleteModelData))]
        public async Task DeleteModel(
            EStatusCode expectedStatus,
            DeleteModel mutation
        )
        {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewModel(id: mutation.Id).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var exists = await MutationsDbContext.Models.AnyAsync(m => m.Id == mutation.Id);
                Assert.False(exists);
            }
        }
    }
}
