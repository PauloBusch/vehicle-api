using Microsoft.EntityFrameworkCore;
using Questor.Vehicle.Domain.Mutations.Users.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using Questor.Vehicle.Domain.Utils.Metadata;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questor.Vehicle.Domain.Mutations.Users.Mutations
{
    public class LoginUser : IMutation
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public async Task<MutationResult> ValidateAsync(VehicleMutationsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Login)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Login)} is require");
            if (string.IsNullOrWhiteSpace(Password)) return new MutationResult(EStatusCode.InvalidData, $"Parameter {nameof(Password)} is require");
            var exists = await handler.DbContext.Users.AnyAsync(u => u.Login == Login);
            if (!exists) return new MutationResult(EStatusCode.Unauthorized, "Login fail");
            return null;
        }

        public async Task<MutationResult> ExecuteAsync(VehicleMutationsHandler handler)
        {
            var user = await handler.DbContext.Users
                .Where(u => u.Login == Login)
                .FirstOrDefaultAsync();

            var valid = user.ValidatePassword(Password);
            if (!valid) return new MutationResult(EStatusCode.Unauthorized, "Login fail");
            var result = new LoginResult { 
                Id = user.Id,
                Name = user.Name,
                Token = new TokenData(user.Id).Generate()
            };
            return new MutationResult(EStatusCode.Success, "Login successfull", result);
        }
    }
}
