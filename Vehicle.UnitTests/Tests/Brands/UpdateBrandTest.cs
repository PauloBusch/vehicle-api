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
            yield return new object[] { EStatusCode.InvalidData, new UpdateBrand { } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateBrand { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(250) } };
            yield return new object[] { EStatusCode.NotFound,    new UpdateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
            yield return new object[] { EStatusCode.Conflict,    new UpdateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
            yield return new object[] { EStatusCode.Success,     new UpdateBrand { Id = RandomId.NewId(), Name = RandomId.NewId(200) } };
        }

        [Theory]
        [MemberData(nameof(UpdateBrandData))]
        public async Task UpdateBrand(
            EStatusCode expectedStatus,
            UpdateBrand mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewBrand(id: mutation.Id, name: mutation.Name).Save();
            if (expectedStatus == EStatusCode.Conflict)
                EntitiesFactory.NewBrand(name: mutation.Name).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var brandDb = await MutationsDbContext.Brands.Where(b => b.Id == mutation.Id).FirstOrDefaultAsync();    
                Assert.Equal(mutation.Name, brandDb.Name);
            }
        }
    }
}
