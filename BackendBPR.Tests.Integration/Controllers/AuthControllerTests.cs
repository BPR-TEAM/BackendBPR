using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BackendBPR.Controllers;
using BackendBPR.Database;
using BackendBPR.Tests.Integration;
 using BackendBPR.Tests.Integration.Utilities;
 using SEP6.Tests.Integration.Utilities;
 using Xunit;

namespace BackendBPR.Tests.Integration.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomApplicationFactory<BackendBPR.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public AuthControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task Register_OK_PasswordHashed()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var login = new User()
            {
                FirstName = "maria",
                Username = "maria",
                Country = "Portugal",
                Email = "string",
                PasswordHash = "string"
            };
            var url = "/Auth/Register";

            var response = await client.PostAsync(url, ResponseHandler<User>.SerializeObject(login));
            var message = await response.Content.ReadAsStringAsync();

            var user = db.Users.FirstOrDefault(a => a.Username == login.Username);
            var salt = user?.PasswordSalt;
            var givenPassword = AuthController.HashPassword(salt, login.PasswordHash);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Registration complete!", message);
            Assert.Equal(user?.PasswordHash, givenPassword);
        }

        [Fact]
        public async Task Login_OK()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var login = new User()
            {
                FirstName = "",
                Username = "",
                Country = "",
                Email = "string",
                PasswordHash = "string",
                Birthday = new DateTime(2000,3,4),
                LastName = "",
            };
            var url = "/Auth/Login";

            var response = await client.PostAsync(url, ResponseHandler<User>.SerializeObject(login));
            var message = await response.Content.ReadAsStringAsync();

            var user = db.Users.FirstOrDefault(a => a.Email == login.Email);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(user.Id + "=" + user.Token, message);
        }

        [Fact]
        public async Task Login_Unauthorized_BlankFields()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var login = new User()
            {
                FirstName = "",
                Username = "",
                Country = "",
                Email = "",
                PasswordHash = "null"
            };
            var url = "/Auth/Login";

            var response = await client.PostAsync(url, ResponseHandler<User>.SerializeObject(login));
            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Fill the fields", message);
        }

        [Fact]
        public async Task Login_Unauthorized_UserDoesNotExist()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var login = new User()
            {
                FirstName = "",
                Username = "",
                Country = "",
                Email = "null",
                PasswordHash = "null"
            };
            var url = "/Auth/Login";

            var response = await client.PostAsync(url, ResponseHandler<User>.SerializeObject(login));
            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("User does not exist", message);
        }

        [Fact]
        public async Task Login_Unauthorized_WrongCredentials()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token", "1=ssss");

            var login = new User()
            {
                FirstName = "",
                Username = "",
                Country = "",
                Email = "string",
                PasswordHash = "stringy"
            };
            var url = "/Auth/Login";

            var response = await client.PostAsync(url, ResponseHandler<User>.SerializeObject(login));
            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Wrong credentials", message);
        }

        [Fact]
        public async Task Logout()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token", "1=ssss");

            var url = "/Auth/Logout";

            var response = await client.PostAsync(url, null);
            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Logout successful", message);
        }
        
        [Fact]
        public async Task Logout_BadRequest()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token", "1=sxcs");

            var url = "/Auth/Logout";

            var response = await client.PostAsync(url, null);
            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("User or token do not exist", message);
        }
    }
}
