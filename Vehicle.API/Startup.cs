using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Questor.Vehicle.API.Authentication;
using Questor.Vehicle.Domain;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Queries;
using System.IO;

namespace Vehicle.API
{
    public class Startup
    {
        public static string Environment { get; set; }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VehicleMutationsDbContext>(options => options
                .UseMySql(Configuration.GetConnectionString("VehicleMutations"))
                .EnableDetailedErrors()
            );

            services.AddDbContext<VehicleQueriesDbContext>(options => options
                .UseMySql(Configuration.GetConnectionString("VehicleQueries"))
                .EnableDetailedErrors()
            );

            services.AddScoped<VehicleMutationsHandler>();
            services.AddScoped<VehicleQueriesHandler>();

            services.AddMvcCore(c => c.EnableEndpointRouting = false)
              .AddApiExplorer()
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
              .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddAuthentication("Authentication")
                .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);

            services.AddAuthorization(options =>
                options.AddPolicy("RequireAuthentication", policy => policy.RequireAuthenticatedUser().Build())
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Environment = env.EnvironmentName;
            VehicleStartup.Configure(
                secret: Configuration.GetValue<string>("Secret"),   
                pathFiles: Path.Combine(env.ContentRootPath, Configuration.GetValue<string>("PathFiles")) 
            );
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseCors("AllowOrigin");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
            app.UseHttpsRedirection();
        }
    }
}
