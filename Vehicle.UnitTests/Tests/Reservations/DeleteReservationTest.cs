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
    public class DeleteReservationTest : TestsBase
    {
        public DeleteReservationTest(VehicleFixture fixture) : base(fixture) { }

        public static IEnumerable<object[]> IgnoreReservationData()
        {
            yield return new object[] { EStatusCode.InvalidData, new DeleteReservation { } };
            yield return new object[] { EStatusCode.NotFound, new DeleteReservation { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new DeleteReservation { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(IgnoreReservationData))]
        public async void IgnoreReservation(
            EStatusCode expectedStatus,
            DeleteReservation mutation
        ) {
            if (expectedStatus != EStatusCode.NotFound)
                EntitiesFactory.NewReservation(id: mutation.Id).Save();

            var result = await MutationsHandler.Handle(mutation);
            Assert.Equal(expectedStatus, result.Status);
            if (expectedStatus == EStatusCode.Success) {
                var exists = await MutationsDbContext.Reservations
                    .AnyAsync(r => r.Id == mutation.Id);
                Assert.False(exists);
            }
        }
    }
}
