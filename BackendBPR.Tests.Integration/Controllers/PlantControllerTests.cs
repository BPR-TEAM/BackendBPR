using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using BackendBPR.Controllers;
using BackendBPR.Database;
using BackendBPR.Tests.Integration.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BackendBPR.Tests.Integration.Controllers
{
    public class PlantControllerTests : IClassFixture<CustomApplicationFactory<BackendBPR.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public PlantControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SearchPlant_OnlyName()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var name = "Common";
            var url = $"/Plant/search?name={name}";

            var response = await client.PostAsync(url,ResponseHandler<List<Tag>>.SerializeObject(new List<Tag>()));
            System.Console.WriteLine(response.Content);
            var searchResults = ResponseHandler<List<string>>.GetObject(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(searchResults);
        }

        [Fact]
        public async Task SearchPlant_OnlyName_NotFound()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var name = "";
            var url = $"/Plant/search?name={name}";

            var response = await client.PostAsync(url,ResponseHandler<List<Tag>>.SerializeObject(new List<Tag>()));

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }    

        [Fact]
        public async Task SearchPlant_TagsNoName()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var tags = new List<Tag>() {new Tag(){Id = 1,Name = "Passion Fruit"}};
            var url = $"/Plant/search";

            var response = await client.PostAsync(url,ResponseHandler<List<Tag>>.SerializeObject(tags));
            System.Console.WriteLine(response.Content);
            var searchResults = ResponseHandler<List<string>>.GetObject(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(searchResults);
        }  

        [Fact]
        public async Task SearchPlant_TagsWithName()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();

            var tags = new List<Tag>() {new Tag(){Id = 1,Name = "Passion Fruit"}};
            var name = "Common";
            var url = $"/Plant/search?name={name}";

            var response = await client.PostAsync(url,ResponseHandler<List<Tag>>.SerializeObject(tags));
            System.Console.WriteLine(response.Content);
            var searchResults = ResponseHandler<List<string>>.GetObject(response);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(searchResults);
        }
    }
    
}