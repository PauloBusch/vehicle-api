using Questor.Vehicle.Domain.Mutations.Models.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System.Collections.Generic;
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
            yield return new object[] { EStatusCode.NotFound,    new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200), BrandId = RandomId.NewId() }, true, false };
            yield return new object[] { EStatusCode.NotFound,    new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200), BrandId = RandomId.NewId() }, false, true };
            yield return new object[] { EStatusCode.Conflict,    new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200), BrandId = RandomId.NewId() }, true, true  };
            yield return new object[] { EStatusCode.Success,     new UpdateModel { Id = RandomId.NewId(), Name = RandomId.NewId(200), BrandId = RandomId.NewId() }, true, true  };
        }


        [Theory]
        [MemberData(nameof(UpdateModelData))]
        public async Task UpdateModel(
            EStatusCode expectedStatus,
            UpdateModel mutation,
            bool? withModel = false,
            bool? withBrand = false
        ) {
            if (withBrand.Value)
                EntitiesFactory.NewBrand(id: mutation.BrandId).Save();
            if (withModel.Value)
                EntitiesFactory.NewModel(id: mutation.Id, name: mutation.Name).Save();
            if (expectedStatus == EStatusCode.Conflict)
                EntitiesFactory.NewModel(name: mutation.Name).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var modelDb = await MutationsDbContext.Models.FindAsync(mutation.Id);
                Assert.Equal(mutation.Name, modelDb.Name);
                Assert.Equal(mutation.BrandId, modelDb.BrandId);
            }
        }
    }
}
