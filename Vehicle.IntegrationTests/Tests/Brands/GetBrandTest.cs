using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Queries.Brands;
using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Brands
{
    public class GetBrandTest : BaseTests
    {
        public GetBrandTest(VehicleFixture fixture) : base(fixture, "/brands") { }

        public static IEnumerable<object[]> GetBrandData()
        {
            yield return new object[] { EStatusCode.NotFound, new GetBrand { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new GetBrand { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(GetBrandData))]
        public async void GetBrad(
            EStatusCode expectedStatus,
            GetBrand query
        ) { 
            var brand = null as Brand;
            if (expectedStatus != EStatusCode.NotFound)
                brand = EntitiesFactory.NewBrand(id: query.Id).Save();
            var (status, result) = await Request.Get<QueryResultOneTest<BrandDetail>>(new Uri($"{Uri}/{query.Id}"), query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var brandResult = result.Data;
                Assert.NotNull(brandResult);
                Assert.Equal(brand.Id, brandResult.Id);
                Assert.Equal(brand.Name, brandResult.Name);
            }
        }
    }
}
