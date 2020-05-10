using Questor.Vehicle.Domain.Mutations.Reservations.Mutations;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.UnitTests.Tests.Reservations
{
    public class FinishReservationTest : TestsBase
    {
        public FinishReservationTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> FinishReservationData()
        {
            yield return new object[] { EStatusCode.InvalidData, new FinishReservation { } };
            yield return new object[] { EStatusCode.InvalidData, new FinishReservation { } };
            yield return new object[] { EStatusCode.InvalidData, new FinishReservation { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.NotFound, new FinishReservation { Id = RandomId.NewId(), DateSale = DateTime.Now } };
            yield return new object[] { EStatusCode.Success, new FinishReservation { Id = RandomId.NewId(), DateSale = DateTime.Now } };
        }

        [Theory]
        [MemberData(nameof(FinishReservationData))]
        public async void FinishReservation(
            EStatusCode expectedStatus,
            FinishReservation mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewReservation(id: mutation.Id).Save();
            
            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) { 
                var reservationDb = await MutationsDbContext.Reservations.FindAsync(mutation.Id);
                Assert.NotNull(reservationDb.DateSale);
                Assert.NotNull(reservationDb.Announcement.DateSale);
            }
        }
    }
}
