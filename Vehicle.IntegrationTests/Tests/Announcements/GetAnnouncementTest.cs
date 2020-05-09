using Questor.Vehicle.Domain.Mutations.Announcements.Entities;
using Questor.Vehicle.Domain.Queries.Announcements;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Announcements
{
    public class GetAnnouncementTest : BaseTests
    {
        public GetAnnouncementTest(VehicleFixture fixture) : base(fixture, "/announcements") { }

        public static IEnumerable<object[]> GetAnnouncementData()
        {
            yield return new object[] { EStatusCode.NotFound, new GetAnnouncement { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new GetAnnouncement { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(GetAnnouncementData))]
        public async void GetAnnouncement(
            EStatusCode expectedStatus,
            GetAnnouncement query
        ) {
            var announcement = null as Announcement;
            if (expectedStatus != EStatusCode.NotFound)
                announcement = EntitiesFactory.NewAnnouncement(id: query.Id).Save();

            var (status, result) = await Request.Get<QueryResultOne<AnnouncementDetail>>(new Uri($"{Uri}/{query.Id}"), query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var announcementResult = result.Data;
                Assert.NotNull(announcementResult);
                Assert.Equal(announcement.PricePurchase, announcementResult.PricePurchase);
                Assert.Equal(announcement.PriceSale, announcementResult.PriceSale);
                Assert.Equal(announcement.DateSale, announcementResult.DateSale);
                Assert.Equal(announcement.VehicleId, announcementResult.VehicleId);
                Assert.Equal(announcement.Vehicle.Year, announcementResult.VehicleYear);
                Assert.Equal(announcement.Vehicle.Model.Name, announcementResult.VehicleModel);
                Assert.Equal(announcement.Vehicle.Brand.Name, announcementResult.VehicleBrand);
            }
        }
    }
}
