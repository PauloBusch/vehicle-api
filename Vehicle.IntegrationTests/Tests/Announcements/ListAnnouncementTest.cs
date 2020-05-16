using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Queries.Announcements;
using Questor.Vehicle.Domain.Queries.Announcements.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vehicle.IntegrationTests.Utils.Results;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Announcements
{
    public class ListAnnouncementTest : BaseTests
    {
        public ListAnnouncementTest(VehicleFixture fixture) : base(fixture, "/announcements") { }

        public static IEnumerable<object[]> ListAnnouncementData()
        {
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { } };
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { Page = 1, Limit = 10 } };
            yield return new object[] { EStatusCode.InvalidData, new ListAnnouncement { Page = 0, Limit = 0 } };
            yield return new object[] { EStatusCode.InvalidData, new ListAnnouncement { Page = 1, Limit = 10, SortOrder = EOrder.Desc, SortColumn = RandomId.NewId() } };
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { Page = 1, Limit = 10, SortOrder = EOrder.Asc, SortColumn = "date_sale" } };
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { Page = 1, Limit = 10, SortOrder = EOrder.Asc, SortColumn = "date_sale", BrandId = RandomId.NewId(), DateSale = DateTime.Now, ModelId = RandomId.NewId(), Year = 2010, ColorId = EColor.Blue } };
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { Page = 1, Limit = 10, SortOrder = EOrder.Asc, SortColumn = "date_sale", BrandId = RandomId.NewId(), DateSale = DateTime.Now, ModelId = RandomId.NewId(), Year = 2010, IncludeSold = true } };
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { Page = 1, Limit = 10, SortOrder = EOrder.Asc, SortColumn = "date_sale", BrandId = RandomId.NewId(), DateSale = DateTime.Now, ModelId = RandomId.NewId(), Year = 2010, IncludeSold = true, IncludeReserved = true } };
            yield return new object[] { EStatusCode.Success,     new ListAnnouncement { Page = 1, Limit = 10, SortOrder = EOrder.Asc, SortColumn = "date_sale", BrandId = RandomId.NewId(), DateSale = DateTime.Now, ModelId = RandomId.NewId(), Year = 2010, IncludeSold = true }, true };
        }

        [Theory]
        [MemberData(nameof(ListAnnouncementData))]
        public async void ListAnnouncement(
            EStatusCode expectedStatus,
            ListAnnouncement query,
            bool? exactlyAnnouncement = false
        ) {
            var vehicle = EntitiesFactory.NewVehicle(
                brandId: query.BrandId, 
                modelId: query.ModelId,
                color: query.ColorId,
                year: query.Year
            ).Save();
            var announcement = EntitiesFactory.NewAnnouncement(
                vehicle: vehicle, 
                dateSale: query.DateSale
            ).Save();
                
            var (status, result) = await Request.Get<QueryResultListTest<AnnouncementList>>(Uri, query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.True(result.TotalRows > 0); 
                Assert.True(result.Data.Length <= query.Limit); 
            }
            if (exactlyAnnouncement.Value)
            {
                var listAnnouncement = result.Data.ToList();
                var announcementResult = listAnnouncement
                    .Where(a => a.Id == announcement.Id)
                    .FirstOrDefault();
                Assert.NotNull(announcementResult);
                Assert.Equal(announcement.PricePurchase, announcementResult.PricePurchase);
                Assert.Equal(announcement.PriceSale, announcementResult.PriceSale);
                Assert.Equal(announcement.DateSale.Value.Date, announcementResult.DateSale.Value.Date);
                Assert.Equal(announcement.Vehicle.Year, announcementResult.VehicleYear);
                Assert.Equal(announcement.Vehicle.Brand.Name, announcementResult.VehicleBrandName);
                Assert.Equal(announcement.Vehicle.Model.Name, announcementResult.VehicleModelName);
            }
        }
    }
}
