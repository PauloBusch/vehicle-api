using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain
{
    public static class VehicleStartup
    {
        internal static string Secret { get; set; }

        public static void Configure(
            string secret    
        ) {
            Secret = secret;

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
