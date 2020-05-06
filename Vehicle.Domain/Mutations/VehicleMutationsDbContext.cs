using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Brands.Entities;
using Questor.Vehicle.Domain.Utils.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations
{
    public class VehicleMutationsDbContext : DbContextBase
    {
        public DbSet<Brand> Brands { get; set; }
        public VehicleMutationsDbContext(DbContextOptions<VehicleMutationsDbContext> options) : base(options) { }
    }
}
