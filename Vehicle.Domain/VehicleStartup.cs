using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain
{
    public static class VehicleStartup
    {
        public static void Configure() {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
