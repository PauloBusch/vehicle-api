using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VehicleMutationsDbContext>(options => options
                .UseMySQL(Configuration.GetConnectionString("VehicleMutations"))
                .EnableDetailedErrors()
            );

            services.AddDbContext<VehicleQueriesDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("VehicleQueries"))
                .EnableDetailedErrors()
            );

            services.AddScoped<VehicleMutationsHandler>();
            services.AddScoped<VehicleQueriesHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseRouting();
            app.UseCors(configure => configure
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
