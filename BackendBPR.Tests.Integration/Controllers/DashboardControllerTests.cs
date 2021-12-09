using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BackendBPR.Database;
using BackendBPR.Tests.Integration.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BackendBPR.Tests.Integration.Controllers
{
    public class DashboardControllerTests : IClassFixture<CustomApplicationFactory<BackendBPR.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public DashboardControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RemovePlants(){           
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");
            var  userPlant =new UserPlant(){Id = 5};
            var id = 1;
            var url = $"Dashboard/plants?id={id}";

            var request = new HttpRequestMessage {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(client.BaseAddress.AbsoluteUri+url),
                    Content = ResponseHandler<UserPlant>.SerializeObject(userPlant)};

            var response = await client.SendAsync(request);           
            var message = await response.Content.ReadAsStringAsync();

            var dashboard = db.Dashboards.Include(d => d.UserPlants).FirstOrDefault(d => d.Id == id);

            Assert.Equal(1,dashboard.UserPlants.Count());
        }






    }
}