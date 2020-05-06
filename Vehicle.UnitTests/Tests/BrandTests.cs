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

namespace Vehicle.UnitTests.Tests
{
    public class BrandTests : TestsBase
    {
        public BrandTests(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> CreateBrandData()
        {
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), null, null };
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), RandomId.NewId(), null };
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), RandomId.NewId(), RandomId.NewId(250) };
            yield return new object[] { new MutationResult(EStatusCode.Conflict), RandomId.NewId(), RandomId.NewId(200) };
            yield return new object[] { new MutationResult(EStatusCode.Success), RandomId.NewId(), RandomId.NewId(200) };
        }

        [Theory]
        [MemberData(nameof(CreateBrandData))]
        public async Task CreateBrand(
            MutationResult expectedResult,
            string id,
            string name
        ) {
            var create = new CreateBrand { Id = id, Name = name };
            if (expectedResult.Status == EStatusCode.Conflict) { 
                var brand = new Brand { Id = RandomId.NewId(), Name = name };
                await MutationsDbContext.Brands.AddAsync(brand);   
            }
            var result = await MutationsHandler.Handle(create);
            Assert.Equal(expectedResult.Status, result.Status);
            if (expectedResult.Status == EStatusCode.Success) { 
                var brandDb = await MutationsDbContext.Brands.Where(b => b.Id == create.Id).FirstOrDefaultAsync();
                Assert.NotNull(brandDb);
                Assert.Equal(create.Name, brandDb.Name);
            }
        }
    }
}
