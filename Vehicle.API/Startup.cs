using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Questor.Vehicle.Domain.Mutations;
using Questor.Vehicle.Domain.Queries;

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

            services.AddDbContext<VehicleQueriesDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("VehicleQueries"))
                .EnableDetailedErrors()
            );

            services.AddScoped<VehicleMutationsHandler>();
            services.AddScoped<VehicleQueriesHandler>();

            services.AddMvcCore(c => c.EnableEndpointRouting = false)
              .AddApiExplorer()
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
              .AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.IgnoreNullValues = true;
              });

            services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Environment = env.EnvironmentName;
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseCors("AllowOrigin");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
            app.UseHttpsRedirection();
        }
    }
}
