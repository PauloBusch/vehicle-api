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
            yield return new object[] { EStatusCode.InvalidData, new CreateModel { } };
            yield return new object[] { EStatusCode.InvalidData, new CreateModel { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new CreateModel { Id = RandomId.NewId(), Name = RandomId.NewId(250) } };
            yield return new object[] { EStatusCode.Conflict,    new CreateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
            yield return new object[] { EStatusCode.Success,     new CreateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
        }

        [Theory]
        [MemberData(nameof(CreateModelData))]
        public async Task CreateModel(
            EStatusCode expectedStatus,
            CreateModel mutation
        ) { 
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
