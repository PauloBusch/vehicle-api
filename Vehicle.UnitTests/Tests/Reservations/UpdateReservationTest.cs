using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Reservations.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Vehicle.UnitTests.Tests.Reservations
{
    public class UpdateReservationTest : TestsBase
    {
        public UpdateReservationTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> UpdateReservationData()
        {
            yield return new object[] { EStatusCode.InvalidData, new UpdateReservation { } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150) } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150) } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(15) } };
            yield return new object[] { EStatusCode.NotFound, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(15), AnnouncementId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(15), AnnouncementId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(20), AnnouncementId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new UpdateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(160), ContactPhone = RandomId.NewId(15), AnnouncementId = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(UpdateReservationData))]
        public async void UpdateReservation(
            EStatusCode expectedStatus,
            UpdateReservation mutation
        ) {

            EntitiesFactory.NewAnnouncement(id: mutation.AnnouncementId).Save();
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewReservation(id: mutation.Id).Save();
            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) {
                var reservationDb = await MutationsDbContext.Reservations
                    .Where(r => r.Id == mutation.Id)
                    .FirstOrDefaultAsync();
                Assert.NotNull(reservationDb);
                Assert.Equal(mutation.AnnouncementId, reservationDb.AnnouncementId);
                Assert.Equal(mutation.ContactName, reservationDb.Contact.Name);
                Assert.Equal(mutation.ContactPhone, reservationDb.Contact.Phone);
            }
        }
    }
}
