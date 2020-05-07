using Questor.Vehicle.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Vehicle.IntegrationTests.Utils;
using Vehicle.UnitTests;
using Xunit;

namespace Vehicle.IntegrationTests.Tests
{
    public class BaseTests : IClassFixture<VehicleFixture>
    {
        protected readonly Uri Uri;
        protected readonly Request Request;
        protected readonly EntitiesFactory EntitiesFactory;
        protected readonly VehicleMutationsDbContext MutationsDbContext;
        protected readonly VehicleMutationsHandler MutationsHandler;
        public BaseTests(VehicleFixture fixture, string url) {
            Request = fixture.Request;
            EntitiesFactory = fixture.EntitiesFactory;
            MutationsDbContext = fixture.MutationsHandler.DbContext;
            MutationsHandler = fixture.MutationsHandler;
            Uri = new Uri($"{fixture.Client.BaseAddress}{url}");
        }
    }
}
