using Questor.Vehicle.Domain.Queries.Announcements;
using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Announcements
{
    public class ListAnnouncementReportTest : BaseTests
    {
        public ListAnnouncementReportTest(VehicleFixture fixture) : base(fixture, null) { }

        public static IEnumerable<object[]> ListAnnouncementReportData()
        {
            yield return new object[] { EStatusCode.Success, new ListAnnouncementReport { } };
            yield return new object[] { EStatusCode.InvalidData, new ListAnnouncementReport { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-1) } };
            yield return new object[] { EStatusCode.Success, new ListAnnouncementReport { StartDate = DateTime.Now } };
            yield return new object[] { EStatusCode.Success, new ListAnnouncementReport { EndDate = DateTime.Now } };
            yield return new object[] { EStatusCode.Success, new ListAnnouncementReport { StartDate = DateTime.Now, EndDate = DateTime.Now } };
        }

        [Theory]
        [MemberData(nameof(ListAnnouncementReportData))]
        public async void ListAnnouncementReport(
            EStatusCode expectedStatus,
            ListAnnouncementReport query
        )
        {
            var announcement = EntitiesFactory.NewAnnouncement(dateSale: DateTime.Now).Save();
            var result = await QueriesHandler.Handle(query);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotEmpty(result.Data);
                var announcementResult = result.Data.FirstOrDefault(a => a.Id == announcement.Id);
                Assert.NotNull(announcementResult);
                Assert.Equal(announcement.PriceSale - announcement.PricePurchase, announcementResult.Profit);
                Assert.Equal(announcement.Vehicle.Brand.Name, announcementResult.VehicleBrand);
                Assert.Equal(announcement.Vehicle.Model.Name, announcementResult.VehicleModel);
            }
        }
    }
}
