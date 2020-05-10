using Questor.Vehicle.Domain.Queries.Reservations;
using Questor.Vehicle.Domain.Queries.Reservations.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Reservations
{
    public class ListReservationsTest : BaseTests
    {
        public ListReservationsTest(VehicleFixture fixture) : base(fixture, "/reservations") { }

        public static IEnumerable<object[]> ListReservationsData()
        {
            yield return new object[] { EStatusCode.Success, new ListReservations { } };
        }

        [Theory]
        [MemberData(nameof(ListReservationsData))]
        public async void ListReservations(
            EStatusCode expectedStatus,
            ListReservations query
        ) {
            var reservation = EntitiesFactory.NewReservation().Save();
            var (status, result) = await Request.Get<QueryResultListTest<ReservationList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var reservationResult = result.Data.FirstOrDefault(r => r.Id == reservation.Id); 
                Assert.NotNull(reservationResult);
                Assert.Equal(reservation.Id, reservationResult.Id);
                Assert.Equal(reservation.Contact.Name, reservationResult.ContactName);
                Assert.Equal(reservation.Contact.Phone, reservationResult.ContactPhone);
                Assert.Equal(reservation.Announcement.Vehicle.Brand.Name, reservationResult.VehicleBrandName);
                Assert.Equal(reservation.Announcement.Vehicle.Model.Name, reservationResult.VehicleModelName);
            }
        }
    }
}
