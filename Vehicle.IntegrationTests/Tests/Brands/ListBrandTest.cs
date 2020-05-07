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
            : base(fixture, "brands") { }

        [Fact]
        public async void ListBrand()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = Uri,
                Method = HttpMethod.Get
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
