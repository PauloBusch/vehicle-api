using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Mutations.Brands.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vehicle.UnitTests.Tests.Brands
{
    public class CreateBrandTest : TestsBase
    {
        public CreateBrandTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> CreateBrandData()
        {
            yield return new object[] { EStatusCode.InvalidData, new CreateBrand {  } };
            yield return new object[] { EStatusCode.InvalidData, new CreateBrand { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new CreateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(250) } };
            yield return new object[] { EStatusCode.Conflict,    new CreateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
            yield return new object[] { EStatusCode.Success,     new CreateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
        }

        [Theory]
        [MemberData(nameof(CreateBrandData))]
        public async Task CreateBrand(
            EStatusCode expectedStatus,
            CreateBrand mutation
        ) {
            if (expectedStatus == EStatusCode.Conflict) 
                EntitiesFactory.NewBrand(name: mutation.Name).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var brandDb = await MutationsDbContext.Brands.Where(b => b.Id == mutation.Id).FirstOrDefaultAsync();
                Assert.NotNull(brandDb);
                Assert.Equal(mutation.Name, brandDb.Name);
            }
        }
    }
}
