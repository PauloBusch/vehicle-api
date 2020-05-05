using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Utils.Database
{
    public abstract class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options) { }

        public async Task<(bool canConnect, string errorMessage)> CheckConnectionAsync()
        {
            try
            {
                var canConnect = await Database.CanConnectAsync();
                if (!canConnect) return (false, null);
                Database.GetDbConnection().Open();
                Database.GetDbConnection().Close();
            }
            catch (SqlException ex)
            {
                return (false, ex.Message);
            }
            return (true, null);
        }
    }
}
