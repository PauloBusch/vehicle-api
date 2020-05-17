using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain
{
    public static class VehicleStartup
    {
        internal static string Secret { get; set; }
        internal static string PathFiles { get; set; }

        public static void Configure(
            string secret,
            string pathFiles
        ) {
            Secret = secret;
            PathFiles = pathFiles;

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
