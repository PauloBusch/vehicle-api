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

namespace Vehicle.IntegrationTests
{
    public class VehicleFixture : IDisposable
    {
        public HttpClient Client { get; }
        public TestServer Server { get; }
        public VehicleMutationsDbContext MutationsDbContext { get; set; }
        public VehicleMutationsHandler MutationsHandler { get; set; }

        private IConfiguration Configuration { get; }
        public VehicleFixture()
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(startupAssembly);
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
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        private void ConfigureServices(IServiceCollection services) {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(startupAssembly);

            var manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(startupAssembly)
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
            MutationsHandler = scopedServices.GetRequiredService<VehicleMutationsHandler>();
            MutationsDbContext = scopedServices.GetRequiredService<VehicleMutationsDbContext>();
            MutationsDbContext.Database.EnsureCreated();
            services.AddSingleton(manager);
        }

        private static string GetProjectPath(Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;
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
