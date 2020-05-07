using Questor.Vehicle.Domain.Mutations.Models.Entities;
using Questor.Vehicle.Domain.Mutations.Models.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vehicle.UnitTests.Tests.Models
{
    public class CreateModelTest : TestsBase
    {
        public CreateModelTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> CreateModelData()
        {
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), null, null };
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), RandomId.NewId(), null };
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), RandomId.NewId(), RandomId.NewId(250) };
            yield return new object[] { new MutationResult(EStatusCode.Conflict), RandomId.NewId(), RandomId.NewId(200) };
            yield return new object[] { new MutationResult(EStatusCode.Success), RandomId.NewId(), RandomId.NewId(200) };
        }


        [Theory]
        [MemberData(nameof(CreateModelData))]
        public async Task CreateModel(
            MutationResult expectedResult,
            string id,
            string name
        ) { 
            if (expectedResult.Status == EStatusCode.Conflict) { 
                var model = new Model { Id = RandomId.NewId(), Name = name };
                await MutationsDbContext.Models.AddAsync(model);
                await MutationsDbContext.SaveChangesAsync();
            }

            var create = new CreateModel { Id = id, Name = name };
            var result = await MutationsHandler.Handle(create);
            Assert.Equal(expectedResult.Status, result.Status);
            if (expectedResult.Status == EStatusCode.Success) { 
                var modelDb = await MutationsDbContext.Models.FindAsync(create.Id);
                Assert.NotNull(modelDb);
                Assert.Equal(create.Name, modelDb.Name);
            }
        }
    }
}
