using Questor.Vehicle.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests
{
    public abstract class TestsBase : IClassFixture<VehicleFixture>
    {
        protected readonly EntitiesFactory EntitiesFactory;
        protected readonly VehicleMutationsDbContext MutationsDbContext;
        protected readonly VehicleMutationsHandler MutationsHandler;
        protected TestsBase(VehicleFixture fixture)
        {
            EntitiesFactory = new EntitiesFactory(fixture.MutationsHandler.DbContext);
            MutationsDbContext = fixture.MutationsHandler.DbContext;
            MutationsHandler = fixture.MutationsHandler;
        }
    }
}
