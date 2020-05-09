using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Announcements.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests.Announcements
{
    public class DeleteAnnouncementTest : TestsBase
    {
        public DeleteAnnouncementTest(VehicleFixture fixture) : base(fixture) { }
    
        public static IEnumerable<object[]> DeleteAnnouncementData()
        {
            yield return new object[] { EStatusCode.InvalidData, new DeleteAnnouncement { } };
            yield return new object[] { EStatusCode.NotFound, new DeleteAnnouncement { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new DeleteAnnouncement { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(DeleteAnnouncementData))]
        public async void DeleteAnnouncement(
            EStatusCode expectedStatus,
            DeleteAnnouncement mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound) 
                EntitiesFactory.NewAnnouncement(id: mutation.Id).Save();
        
            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var existsAnnouncement = await MutationsDbContext.Announcements.AnyAsync(a => a.Id == mutation.Id);
                Assert.False(existsAnnouncement);
            }
        }
    }
}
