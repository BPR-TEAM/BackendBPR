using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BackendBPR.Controllers;
using BackendBPR.Database;
using BackendBPR.Tests.Integration;
 using BackendBPR.Tests.Integration.Utilities;
 using Xunit;

namespace BackendBPR.Tests.Integration.Controllers
{
    public class AdviceControllerTests : IClassFixture<CustomApplicationFactory<BackendBPR.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public AdviceControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetDefaultAdvice()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();            

            var url = "/Advice/default";

            var response = await client.GetAsync(url);
            var result = ResponseHandler<List<Advice>>.GetObject(response);

            var advice = db.Advices
            .Where(advice => advice.TagId == null).ToList();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(advice[0].Id, result[0].Id);
        }

        [Fact]
        public async Task GetAdvice()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");            

            var id = 1;
            var url = $"/Advice?plantId={id}";

            var response = await client.GetAsync(url);
            var result = ResponseHandler<List<Advice>>.GetObject(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(result[0].Tag.Plants.Any(p => p.Id == id));
        }

        [Theory]             
        [InlineData(AdviceRole.Like,1,"")]
        [InlineData(AdviceRole.Dislike,2,"UserAdvice type updated")]
        [InlineData(AdviceRole.Like,3,"UserAdvice removed")] 
        [InlineData(AdviceRole.Like,4,"UserAdvice added")]     
        public async Task Vote_Like_Dislike(AdviceRole vote,int adviceId, string messageExpected)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();
            var userId = 1;
            client.DefaultRequestHeaders.Add("token",$"{userId}=ssss");

            var url = $"/Advice?adviceId={adviceId}&vote={vote}";

            var response = await client.PutAsync(url,null);
            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(messageExpected,message);
        }
    }
}