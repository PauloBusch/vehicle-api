using Questor.Vehicle.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests
{
    public class BaseTests : IClassFixture<VehicleFixture>
    {
        protected readonly VehicleMutationsDbContext MutationsDbContext;
        protected readonly VehicleMutationsHandler MutationsHandler;
        public BaseTests(VehicleFixture fixture) {
            MutationsDbContext = fixture.MutationsHandler.DbContext;
            MutationsHandler = fixture.MutationsHandler;
        }
    }
}
