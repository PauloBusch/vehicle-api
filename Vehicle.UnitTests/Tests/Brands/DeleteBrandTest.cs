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
            yield return new object[] { EStatusCode.InvalidData, new DeleteBrand { } };
            yield return new object[] { EStatusCode.NotFound,    new DeleteBrand { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success,     new DeleteBrand { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(DeleteBrandData))]
        public async Task DeleteBrand(
            EStatusCode expectedStatus,
            DeleteBrand mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewBrand(id: mutation.Id).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var exists = await MutationsDbContext.Brands.AnyAsync(b => b.Id == mutation.Id);    
                Assert.False(exists);  
            }
        }
    }
}
