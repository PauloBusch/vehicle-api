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
    public class UpdateBrandTest : TestsBase
    {
        public UpdateBrandTest(VehicleFixture fixture) : base(fixture) { }
        
        public static IEnumerable<object[]> UpdateBrandData()
        {
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), null, null };
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), RandomId.NewId(), null };
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), RandomId.NewId(), RandomId.NewId(250) };
            yield return new object[] { new MutationResult(EStatusCode.NotFound), RandomId.NewId(), RandomId.NewId(200) };
            yield return new object[] { new MutationResult(EStatusCode.Conflict), RandomId.NewId(), RandomId.NewId(200) };
            yield return new object[] { new MutationResult(EStatusCode.Success), RandomId.NewId(), RandomId.NewId(200) };
        }

        [Theory]
        [MemberData(nameof(UpdateBrandData))]
        public async Task UpdateBrand(
            MutationResult expectedResult,
            string id,
            string name
        ) {
            if (expectedResult.Status != EStatusCode.NotFound) { 
                var brand = new Brand { Id = id ?? RandomId.NewId(), Name = name };
                await MutationsDbContext.Brands.AddAsync(brand);
                await MutationsDbContext.SaveChangesAsync();
            }
            if (expectedResult.Status == EStatusCode.Conflict)
            {
                var brand = new Brand { Id = RandomId.NewId(), Name = name };
                await MutationsDbContext.Brands.AddAsync(brand);
                await MutationsDbContext.SaveChangesAsync();
            }

            var update = new UpdateBrand { Id = id, Name = name };
            var result = await MutationsHandler.Handle(update);
            Assert.Equal(expectedResult.Status, result.Status);
            if (expectedResult.Status == EStatusCode.Success) { 
                var brandDb = await MutationsDbContext.Brands.Where(b => b.Id == update.Id).FirstOrDefaultAsync();    
                Assert.Equal(update.Name, brandDb.Name);
            }
        }
    }
}
