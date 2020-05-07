using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Vehicle.API;
using Vehicle.IntegrationTests.Utils;
using Vehicle.UnitTests;

namespace Vehicle.IntegrationTests
{
    public class VehicleFixture : IDisposable
    {
        public Request Request { get; private set; }
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }
        public EntitiesFactory EntitiesFactory { get; private set; }
        public VehicleMutationsDbContext MutationsDbContext { get; private set; }
        public VehicleMutationsHandler MutationsHandler { get; private set; }

        private IConfiguration Configuration { get; }
        public VehicleFixture()
        {
            var contentRoot = GetProjectPath();
            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(ConfigureServices)
                .UseConfiguration(Configuration)
                .UseEnvironment("Test")
                .UseStartup(typeof(Startup));

            Server = new TestServer(webHostBuilder);

            Client = Server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost:44349/api/vehicle");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Request = new Request(Client);
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        private void ConfigureServices(IServiceCollection services) {
            var contentRoot = GetProjectPath();

            var manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(typeof(Startup).GetTypeInfo().Assembly)
                },
                FeatureProviders =
                {
                    new ControllerFeatureProvider(),
                    new ViewComponentFeatureProvider()
                }
            };

            services.AddDbContext<VehicleMutationsDbContext>(options => options
                .UseMySql(Configuration.GetConnectionString("VehicleMutations"))
                .EnableDetailedErrors()
            );

            services.AddDbContext<VehicleQueriesDbContext>(options => options
                .UseMySql(Configuration.GetConnectionString("VehicleQueries"))
                .EnableDetailedErrors()
            );

            var scope = services
                .BuildServiceProvider()
                .CreateScope();

            var scopedServices = scope.ServiceProvider;
            MutationsDbContext = scopedServices.GetRequiredService<VehicleMutationsDbContext>();
            MutationsHandler = new VehicleMutationsHandler(MutationsDbContext);
            EntitiesFactory = new EntitiesFactory(MutationsDbContext);
            MutationsDbContext.Database.EnsureCreated();
            services.AddSingleton(manager);
        }

        private static string GetProjectPath()
        {
            var projectName = "Vehicle.API";
            var applicationBasePath = AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;
                var projectDirectoryInfo = new DirectoryInfo(directoryInfo.FullName);
                if (projectDirectoryInfo.Exists)
                    if (new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj")).Exists)
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
            }
            while (directoryInfo.Parent != null);
            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }
    }
}
