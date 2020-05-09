using Questor.Vehicle.Domain.Queries.Brands;
using Questor.Vehicle.Domain.Queries.Brands.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Brands
{
    public class ListBrandsTest : BaseTests
    {
        public ListBrandsTest(VehicleFixture fixture) 
            : base(fixture, "/brands") { }

        public static IEnumerable<object[]> ListBrandsData()
        {
            yield return new object[] { EStatusCode.Success, new ListBrands { } };
        }

        [Theory]
        [MemberData(nameof(ListBrandsData))]
        public async void ListBrands(
            EStatusCode expectedStatus,
            ListBrands query
        ) {
            var brand = EntitiesFactory.NewBrand().Save();
            var (status, result) = await Request.Get<QueryResultList<Brand>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotEmpty(result.Data);
                Assert.Contains(result.Data, d => d.Id == brand.Id);
            }
        }
    }
}
