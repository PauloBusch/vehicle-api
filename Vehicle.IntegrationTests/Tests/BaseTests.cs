using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Queries;
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
        protected readonly VehicleQueriesHandler QueriesHandler;
        public BaseTests(VehicleFixture fixture, string url) {
            Request = fixture.Request;
            EntitiesFactory = fixture.EntitiesFactory;
            MutationsDbContext = fixture.MutationsHandler.DbContext;
            MutationsHandler = fixture.MutationsHandler;
            QueriesHandler = fixture.QueriesHandler;
            Uri = new Uri($"{fixture.Client.BaseAddress}{url}");
        }
    }
}
