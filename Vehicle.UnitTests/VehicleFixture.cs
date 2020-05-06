using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain;
using Questor.Vehicle.Domain.Mutations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests
{
    public class VehicleFixture : IDisposable
    {
        public readonly VehicleMutationsHandler MutationsHandler;
        public readonly EntitiesFactory EntitiesFactory;
        public VehicleFixture()
        {
            var builder = new DbContextOptionsBuilder<VehicleMutationsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableDetailedErrors();
            var mutationsDbContext = new VehicleMutationsDbContext(builder.Options);
            MutationsHandler = new VehicleMutationsHandler(mutationsDbContext);
            EntitiesFactory = new EntitiesFactory();

            VehicleStartup.Configure();
        }

        public void Dispose()
        {
            MutationsHandler.DbContext.Dispose();
        }
    }
}
