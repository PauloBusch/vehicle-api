using Questor.Vehicle.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests
{
    public abstract class TestsBase : IClassFixture<VehicleFixture>
    {
        protected readonly VehicleMutationsDbContext MutationsDbContext;
        protected readonly VehicleMutationsHandler MutationsHandler;
        protected readonly EntitiesFactory EntitiesFactory;
        protected TestsBase(VehicleFixture fixture)
        {
            MutationsDbContext = fixture.MutationsHandler.DbContext;
            MutationsHandler = fixture.MutationsHandler;
            EntitiesFactory = fixture.EntitiesFactory;
        }
    }
}
