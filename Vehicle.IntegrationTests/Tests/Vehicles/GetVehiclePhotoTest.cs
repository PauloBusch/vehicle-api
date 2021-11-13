using Questor.Vehicle.Domain.Mutations.Vehicles.Entities.Enums;
using Questor.Vehicle.Domain.Mutations.Vehicles.Mutations;
using Questor.Vehicle.Domain.Queries.Vehicles;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Files;
using Questor.Vehicle.Domain.Utils.Random;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Vehicles
{
    public class GetVehiclePhotoTest : BaseTests
    {
        public GetVehiclePhotoTest(VehicleFixture fixture) : base(fixture, "/vehicles") { }

        public static IEnumerable<object[]> GetVehiclePhotoData()
        {
            yield return new object[] { EStatusCode.NotFound, new GetVehiclePhoto { Id = RandomId.NewId() }, false };
            yield return new object[] { EStatusCode.Success, new GetVehiclePhoto { Id = RandomId.NewId() }, true };
        }

        [Theory]
        [MemberData((nameof(GetVehiclePhotoData)))]
        public async void GetVehiclePhoto(
            EStatusCode expectedStatus,
            GetVehiclePhoto query,
            bool? withFile = false
        ) {
            if (withFile.Value) 
                await CreateVehicleWithImageAsync(query.Id);

            var (status, file) = await Request.DownloadFile(new Uri($"{Uri}/{query.Id}/photo"), $"{query.Id}.jpg", query);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                Assert.NotNull(file);    
                Assert.True(file.Exists);
            }
        }

        private async Task CreateVehicleWithImageAsync(string id)
        {
            var image = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Bmp);
            byte[] imageBytes = stream.ToArray();
            var model = EntitiesFactory.NewModel().Save();
            var createVehicle = new CreateVehicle
            {
                Id = id ?? RandomId.NewId(),
                FuelId = EFuel.Flex,
                ColorId = EColor.Black,
                Year = 2010,
                ModelId = model.Id,
                Board = RandomId.NewId()
            };
            createVehicle.ImageBase64 = Convert.ToBase64String(imageBytes);
            await MutationsHandler.Handle(createVehicle);

        }
    }
}
