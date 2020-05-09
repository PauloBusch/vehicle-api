using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Vehicles.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests.Vehicles
{
    public class SellVehicleTest : TestsBase
    {
        public SellVehicleTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> SellVehicleData()
        {
            yield return new object[] { EStatusCode.InvalidData, new SellVehicle { } };
            yield return new object[] { EStatusCode.InvalidData, new SellVehicle { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.NotFound, new SellVehicle { Id = RandomId.NewId(), DateSale = DateTime.Now }, false, false };
            yield return new object[] { EStatusCode.NotFound, new SellVehicle { Id = RandomId.NewId(), DateSale = DateTime.Now }, true, false };
            yield return new object[] { EStatusCode.NotFound, new SellVehicle { Id = RandomId.NewId(), DateSale = DateTime.Now }, false, true };
            yield return new object[] { EStatusCode.Success, new SellVehicle { Id = RandomId.NewId(), DateSale = DateTime.Now }, true, true };
        }

        [Theory]
        [MemberData(nameof(SellVehicleData))]
        public async void SellVehicle(
            EStatusCode expectedStatus,
            SellVehicle mutation,
            bool? withVehicle = false,
            bool? withAnnouncement = false
        ) {
            if (withVehicle.Value)
                EntitiesFactory.NewVehicle(id: mutation.Id).Save();
            if (withAnnouncement.Value)
                EntitiesFactory.NewAnnouncement(vehicleId: mutation.Id).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var announcementDb = await MutationsDbContext.Announcements
                    .Where(a => a.VehicleId == mutation.Id)
                    .FirstOrDefaultAsync();

                Assert.NotNull(announcementDb);
                Assert.NotNull(announcementDb.DateSale);
                Assert.Equal(mutation.DateSale.Value.Date, announcementDb.DateSale.Value.Date);
            }
        }
    }
}
