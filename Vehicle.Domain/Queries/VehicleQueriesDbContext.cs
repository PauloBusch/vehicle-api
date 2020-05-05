using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Utils.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Queries
{
    public class VehicleQueriesDbContext : DbContextBase
    {
        public VehicleQueriesDbContext(DbContextOptions<VehicleQueriesDbContext> options) : base(options) { }
    }
}
