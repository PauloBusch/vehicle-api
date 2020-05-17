using Questor.Vehicle.Domain.Queries.Announcements;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Announcements
{
    public class ListAnnouncementSelectTest : BaseTests
    {
        public ListAnnouncementSelectTest(VehicleFixture fixture) : base(fixture, "/announcements/select") { }

        public static IEnumerable<object[]> ListAnnouncementSelectData()
        {
            yield return new object[] { EStatusCode.Success, new ListAnnouncementSelect{ } };
            yield return new object[] { EStatusCode.Success, new ListAnnouncementSelect{ IncludeReserved=true } };
        }

        [Theory]
        [MemberData(nameof(ListAnnouncementSelectData))]
        public async void ListAnnouncementSelect(
            EStatusCode expectedStatus, 
            ListAnnouncementSelect query
        ) {
            var announcement = EntitiesFactory.NewAnnouncement().Save();
            var (status, result) = await Request.Get<QueryResultListTest<AnnouncementSelectList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var announcementResult = result.Data.FirstOrDefault(f => f.Id == announcement.Id);
                Assert.NotNull(announcementResult);
            }
        }
    }
}
