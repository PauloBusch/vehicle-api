using Questor.Vehicle.Domain.Queries.Models.ViewModels;
using Questor.Vehicle.Domain.Queries.Models;
using Questor.Vehicle.Domain.Utils.Results;
using Questor.Vehicle.Domain.Utils.Enums;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Models
{
    public class ListModelsTest : BaseTests
    {
        public ListModelsTest(VehicleFixture fixture) : base(fixture, "/models") { }
    
        [Fact]
        public async void ListModels()
        {
            var query = new ListModels();
            var model = EntitiesFactory.NewModel().Save();
            var (status, result) = await Request.Get<QueryResultList<Model>>(Uri, query);
            Assert.Equal(EStatusCode.Success, status);
            Assert.NotEmpty(result.Data);
            Assert.Contains(result.Data, d => d.Id == model.Id);
        }
    }
}
