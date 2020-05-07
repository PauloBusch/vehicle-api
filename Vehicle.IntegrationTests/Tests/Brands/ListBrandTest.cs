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
    public class ListBrandTest : BaseTests
    {
        public ListBrandTest(VehicleFixture fixture) 
            : base(fixture, "/brands") { }

        [Fact]
        public async void ListBrand()
        {
            var query = new ListBrands();
            var (status, result) = await Request.Get<QueryResult<Brand>>(Uri, query);
            Assert.Equal(EStatusCode.Success, status);
        }
    }
}
