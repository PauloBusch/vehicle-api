using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Reservations.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests.Reservations
{
    public class CreateReservationTest : TestsBase
    {
        public CreateReservationTest(VehicleFixture fixture) : base(fixture) { }
    
        public static IEnumerable<object[]> CreateReservationData()
        {
            yield return new object[] { EStatusCode.InvalidData, new CreateReservation { } };
            yield return new object[] { EStatusCode.InvalidData, new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150) } };
            yield return new object[] { EStatusCode.InvalidData, new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150) } };
            yield return new object[] { EStatusCode.InvalidData, new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(15) } };
            yield return new object[] { EStatusCode.NotFound,    new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(15), AnnouncementId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success,     new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(15), AnnouncementId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(150), ContactPhone = RandomId.NewId(20), AnnouncementId = RandomId.NewId() } };
            yield return new object[] { EStatusCode.InvalidData, new CreateReservation { Id = RandomId.NewId(), ContactName = RandomId.NewId(160), ContactPhone = RandomId.NewId(15), AnnouncementId = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(CreateReservationData))]
        public async void CreateReservation(
            EStatusCode expectedStatus,
            CreateReservation mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewAnnouncement(id: mutation.AnnouncementId).Save();

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
