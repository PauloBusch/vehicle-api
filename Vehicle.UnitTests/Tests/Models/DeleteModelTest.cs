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
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), null };
            yield return new object[] { new MutationResult(EStatusCode.NotFound), RandomId.NewId() };
            yield return new object[] { new MutationResult(EStatusCode.Success), RandomId.NewId() };
        }

        [Theory]
        [MemberData(nameof(DeleteModelData))]
        public async Task DeleteModel(
            MutationResult expectedResult,
            string id
        )
        {
            if (expectedResult.Status != EStatusCode.NotFound) { 
                var model = new Model { Id = id ?? RandomId.NewId(), Name = RandomId.NewId(200) };
                await MutationsDbContext.AddAsync(model);
                await MutationsDbContext.SaveChangesAsync();
            }
            var delete = new DeleteModel { Id = id };
            var result = await MutationsHandler.Handle(delete);
            Assert.Equal(expectedResult.Status, result.Status);
            if (expectedResult.Status == EStatusCode.Success) { 
                var exists = await MutationsDbContext.Models.AnyAsync(m => m.Id == delete.Id);
                Assert.False(exists);
            }
        }
    }
}
