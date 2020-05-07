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
    public class DeleteBrandTest : TestsBase
    {
        public DeleteBrandTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> DeleteBrandData()
        {
            yield return new object[] { new MutationResult(EStatusCode.InvalidData), null };
            yield return new object[] { new MutationResult(EStatusCode.NotFound), RandomId.NewId() };
            yield return new object[] { new MutationResult(EStatusCode.Success), RandomId.NewId() };
        }

        [Theory]
        [MemberData(nameof(DeleteBrandData))]
        public async Task DeleteBrand(
            MutationResult expectedResult,
            string id
        ) {
            if (expectedResult.Status != EStatusCode.NotFound) { 
                var brand = new Brand { Id = id ?? RandomId.NewId(), Name = RandomId.NewId(200) };
                await MutationsDbContext.Brands.AddAsync(brand);
                await MutationsDbContext.SaveChangesAsync();
            }

            var delete = new DeleteBrand { Id = id };
            var result = await MutationsHandler.Handle(delete);
            Assert.Equal(expectedResult.Status, result.Status);
            if (expectedResult.Status == EStatusCode.Success) { 
                var exists = await MutationsDbContext.Brands.AnyAsync(b => b.Id == delete.Id);    
                Assert.False(exists);  
            }
        }
    }
}
