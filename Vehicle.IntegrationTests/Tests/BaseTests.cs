using Questor.Vehicle.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests
{
    public class BaseTests : IClassFixture<VehicleFixture>
    {
        protected readonly Uri Uri;
        protected readonly HttpClient Client;
        protected readonly VehicleMutationsDbContext MutationsDbContext;
        protected readonly VehicleMutationsHandler MutationsHandler;
        public BaseTests(VehicleFixture fixture, string url) {
            Client = fixture.Client;
            MutationsDbContext = fixture.MutationsHandler.DbContext;
            MutationsHandler = fixture.MutationsHandler;
            Uri = new Uri($"{Client.BaseAddress}{url}");
        }
    }
}
