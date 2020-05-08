using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations
{
    public class VehicleMutationsDbContext : DbContextBase
    {
        public DbSet<Brands.Entities.Brand> Brands { get; set; }
        public DbSet<Models.Entities.Model> Models { get; set; }
        public DbSet<Vehicles.Entities.Vehicle> Vehicles { get; set; }
        public VehicleMutationsDbContext(DbContextOptions<VehicleMutationsDbContext> options) : base(options) { }
    }
}
