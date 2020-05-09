using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Announcements.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests.Announcements
{
    public class UpdateAnnouncementTest : TestsBase
    {
        public UpdateAnnouncementTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> UpdateAnnouncementData()
        {
            yield return new object[] { EStatusCode.InvalidData, new UpdateAnnouncement { } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateAnnouncement { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000 } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000 } };
            yield return new object[] { EStatusCode.NotFound,    new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000, VehicleId = RandomId.NewId() }, false, true };
            yield return new object[] { EStatusCode.Success,     new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000, VehicleId = RandomId.NewId() }, true, true };
            yield return new object[] { EStatusCode.NotFound,    new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000, VehicleId = RandomId.NewId() }, true, false };
            yield return new object[] { EStatusCode.InvalidData, new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 0, PriceSale = 35000, VehicleId = RandomId.NewId() }, true, true };
            yield return new object[] { EStatusCode.InvalidData, new UpdateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 0, VehicleId = RandomId.NewId() }, true, true };
        }

        [Theory]
        [MemberData(nameof(UpdateAnnouncementData))]
        public async void UpdateAnnouncement(
            EStatusCode expectedStatus,
            UpdateAnnouncement mutation,
            bool? withVehicle = false,
            bool? withAnnouncement = false
        ) {
            if (withVehicle.Value)
                EntitiesFactory.NewVehicle(id: mutation.VehicleId).Save();
            if (withAnnouncement.Value)
                EntitiesFactory.NewAnnouncement(id: mutation.Id).Save();
            
            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) {
                var announcementDb = await MutationsDbContext.Announcements
                    .Where(a => a.Id == mutation.Id)
                    .FirstOrDefaultAsync();
                Assert.NotNull(announcementDb);
                Assert.Equal(mutation.PricePurchase, announcementDb.PricePurchase);
                Assert.Equal(mutation.PriceSale, announcementDb.PriceSale);
                Assert.Equal(mutation.VehicleId, announcementDb.Vehicle.Id);
                Assert.Null(announcementDb.DateSale);
            }
        }
    }
}
