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
using Microsoft.EntityFrameworkCore;

namespace BackendBPR.Tests.Integration.Controllers
{
    public class MyPlantControllerTests : IClassFixture<CustomApplicationFactory<BackendBPR.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public MyPlantControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ImportData(){
             using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrangeBushContext>();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");

            var  csv = "CO2,Date,Humidity,Date\n" + 
                       "5,4/4/2021,50,4/4/2021\n" +
                       "4,4/4/2021,60,4/4/2021\n"  +
                       "3,4/4/2021,70,4/4/2021";

            var userPlantId = 5;
            var url = $"/Plant/MyPlant/{userPlantId}";
            
            var response = await client.PostAsync(url, ResponseHandler<string>.SerializeObject(csv));           
            var message = await response.Content.ReadAsStringAsync();  

            var measurements = db.Measurements.Include(m=>m.MeasurementDefinition).AsNoTracking().AsParallel().ToList();

            Assert.Equal(6,measurements.Count());
            Assert.Equal(5,measurements[0].UserPlantId);
            Assert.Equal("5",measurements[0].Value);
            Assert.Equal("CO2",measurements[0].MeasurementDefinition.Name);
            Assert.Equal("50",measurements[1].Value);            
            Assert.Equal(5,measurements[1].UserPlantId);       
            Assert.Equal("Humidity",measurements[1].MeasurementDefinition.Name);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);     
            Assert.Equal("Data is saved properly",message);
        }


    }
}