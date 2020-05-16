using Questor.Vehicle.Domain.Mutations.Reservations.Entities;
using Questor.Vehicle.Domain.Queries.Reservations;
using Questor.Vehicle.Domain.Queries.Reservations.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Reservations
{
    public class GetReservationTest : BaseTests
    {
        public GetReservationTest(VehicleFixture fixture) : base(fixture, "/reservations") { }

        public static IEnumerable<object[]> GetReservationData()
        {
            yield return new object[] { EStatusCode.NotFound, new GetReservation { Id = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success, new GetReservation { Id = RandomId.NewId() } };
        }

        [Theory]
        [MemberData(nameof(GetReservationData))]
        public async void GetReservation(
            EStatusCode expectedStatus,
            GetReservation query
        ) {
            var reservation = null as Reservation;
            if (expectedStatus != EStatusCode.NotFound)
                reservation = EntitiesFactory.NewReservation(id: query.Id).Save();

            var (status, result) = await Request.Get<QueryResultOneTest<ReservationDetail>>(new Uri($"{Uri}/{query.Id}"), query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var reservationResult = result.Data;
                Assert.NotNull(reservationResult);
                var vehicle = reservation.Announcement.Vehicle;
                var expectedName = $"{vehicle.Model.Brand.Name} - {vehicle.Model.Name}";
                Assert.Equal(reservation.Id, reservationResult.Id);
                Assert.Equal(reservation.Contact.Name, reservationResult.ContactName);
                Assert.Equal(reservation.Contact.Phone, reservationResult.ContactPhone);
                Assert.Equal(reservation.AnnouncementId, reservationResult.AnnouncementId);
                Assert.Equal(expectedName, reservationResult.AnnouncementName);
            }
        }
    }
}
