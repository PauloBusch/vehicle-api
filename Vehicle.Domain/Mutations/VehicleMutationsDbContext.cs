using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations
{
    public class VehicleMutationsDbContext : DbContextBase
    {
        public VehicleMutationsDbContext(DbContextOptions<VehicleMutationsDbContext> options) : base(options) { }
    }
}
