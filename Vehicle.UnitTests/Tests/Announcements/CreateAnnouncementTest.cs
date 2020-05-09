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
    public class CreateAnnouncementTest : TestsBase
    {
        public CreateAnnouncementTest(VehicleFixture fixture) : base(fixture) { }
    
        public static IEnumerable<object[]> CreateAnnouncementData()
        {
            yield return new object[] { EStatusCode.InvalidData, new CreateAnnouncement { } };
            yield return new object[] { EStatusCode.InvalidData, new CreateAnnouncement { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new CreateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000 } };
            yield return new object[] { EStatusCode.InvalidData, new CreateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000 } };
            yield return new object[] { EStatusCode.NotFound,    new CreateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000, VehicleId = RandomId.NewId() }, false };
            yield return new object[] { EStatusCode.Success,     new CreateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 35000, VehicleId = RandomId.NewId() }, true };
            yield return new object[] { EStatusCode.InvalidData, new CreateAnnouncement { Id = RandomId.NewId(), PricePurchase = 0, PriceSale = 35000, VehicleId = RandomId.NewId() }, true };
            yield return new object[] { EStatusCode.InvalidData, new CreateAnnouncement { Id = RandomId.NewId(), PricePurchase = 30000, PriceSale = 0, VehicleId = RandomId.NewId() }, true };
        }

        [Theory]
        [MemberData(nameof(CreateAnnouncementData))]
        public async void CreateAnnouncement(
            EStatusCode expectedStatus,
            CreateAnnouncement mutation,
            bool? withVehicle = false 
        ) { 
            if (withVehicle.Value) 
                EntitiesFactory.NewVehicle(id: mutation.VehicleId).Save();
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
