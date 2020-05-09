using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Mutations.Users.ViewModels
{
    public class LoginResult : IViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
