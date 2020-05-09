using Questor.Vehicle.Domain.Mutations.Users.Entities;
using Questor.Vehicle.Domain.Mutations.Users.Mutations;
using Questor.Vehicle.Domain.Mutations.Users.ViewModels;
using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Random;
using Questor.Vehicle.Domain.Utils.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vehicle.IntegrationTests.Tests.Users
{
    public class LoginUserTest : BaseTests
    {
        public LoginUserTest(VehicleFixture fixture) : base(fixture, "/users/login") { }
    
        public static IEnumerable<object[]> LoginUserData()
        {
            yield return new object[] { EStatusCode.InvalidData,  new LoginUser { } };
            yield return new object[] { EStatusCode.InvalidData,  new LoginUser { Login = RandomId.NewId(50) } };
            yield return new object[] { EStatusCode.Unauthorized, new LoginUser { Login = RandomId.NewId(50), Password = RandomId.NewId(30) }, false };
            yield return new object[] { EStatusCode.Unauthorized, new LoginUser { Login = RandomId.NewId(50), Password = RandomId.NewId(15) }, true, false };
            yield return new object[] { EStatusCode.Success,      new LoginUser { Login = RandomId.NewId(50), Password = RandomId.NewId(10) }, true, true };
            yield return new object[] { EStatusCode.Success,      new LoginUser { Login = RandomId.NewId(50), Password = RandomId.NewId(50) }, true, true };
        }

        [Theory]
        [MemberData(nameof(LoginUserData))]
        public async void LoginUser(
            EStatusCode expectedStatus,
            LoginUser mutation,
            bool? withUser,
            bool? equalPassword
        ) { 
            var user = null as User;
            if (withUser.Value) { 
                EntitiesFactory.NewUser(
                    login: mutation.Login, 
                    password: equalPassword.Value ? mutation.Password : null
                ).Save();
            }
            var (status, result) = await Request.Post<MutationResult>(Uri, mutation);
            Assert.Equal(expectedStatus, status);
            if (expectedStatus == EStatusCode.Success) { 
                var loginResult = result.Data as LoginResult;
                Assert.NotNull(loginResult);
                Assert.NotNull(loginResult.Token);
                Assert.StartsWith("Bearer ", loginResult.Token);
                Assert.Equal(user.Id, loginResult.Id);
                Assert.Equal(user.Name, loginResult.Name);
            }
        }
    }
}
