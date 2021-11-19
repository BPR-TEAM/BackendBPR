using System;
using System.Collections.Generic;
using System.Linq;
using BackendBPR.Database;
using BackendBPR.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendBPR.Controllers
{
    /// <summary>
    /// Controller for personal plants
    /// </summary>
    [ApiController]
    [Route("Plant/[controller]")]
    public class MyPlantController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;

        /// <summary>
        /// Constructor for the controller
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="db">Database</param>
        public MyPlantController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        /// <summary>
        /// User takes an existent plant
        /// </summary>
        /// <param name="token">User token</param>
        /// <param name="plant">Userplant - does not require user ID</param>
        /// <returns>Success message</returns>
        [HttpPost]
        public ObjectResult TakePlant([FromHeader] string token, [FromBody] UserPlant plant)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");
            
            plant.UserId = user.Id;
            
           _dbContext.UserPlants.Add(plant);
           _dbContext.SaveChanges();

           return Ok("Plant added");
        }


        /// <summary>
        /// Get my plant
        /// </summary>
        /// <param id="plantId"></param>
        /// <returns>The plant</returns>
        [HttpGet]
        public ObjectResult GetMyPlant([FromHeader] string token, int id)
        {            
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

           return Ok(_dbContext.UserPlants
                    .Include(p => p.Measurements)
                    .AsNoTracking()
                    .AsParallel()
                    .FirstOrDefault(a => a.PlantId == id));
        }

       /// <summary>
       /// Edit plant info
       /// </summary>
       /// <param name="plant">User plant</param>
       /// <param name="token">authentication token</param>
       /// <returns>user plant</returns>
        [HttpPut]
        public ObjectResult EditPlantInfo([FromHeader] string token, [FromBody] UserPlant plant)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

           var userPlant = _dbContext.UserPlants
           .AsParallel()
           .FirstOrDefault(a => a.PlantId == plant.Id);

           userPlant.Image = plant.Image;
           userPlant.Name = plant.Name;

           _dbContext.SaveChanges();

            return Ok("Changes saved");
        }

        /// <summary>
        /// Remove my plant
        /// </summary>
        /// <param name="userPlantId"></param>
        /// <param name="token">authentication token</param>
        /// <returns>Success message</returns>
        [HttpDelete]
        public ObjectResult RemoveMyPlant([FromHeader] string token, int userPlantId)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

           var userPlant = _dbContext.UserPlants
           .AsParallel()
           .FirstOrDefault(a => a.PlantId == userPlantId);

           _dbContext.Remove(userPlant);
           _dbContext.SaveChanges();

            return Ok("");
        }

         /// <summary>
        /// Get measurements of a plant
        /// </summary>
        /// <param name="userPlantId"></param>
        /// <param name="token">authentication token</param>
        /// <returns>The plant measurements</returns>
        [HttpGet]        
        [Route("{userPlantId}")]
        public ObjectResult GetMeasurements([FromHeader] string token, int userPlantId)
        {
           if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

           return Ok(_dbContext.Measurements.Where(b => b.UserPlantId == userPlantId));
        }

        /// <summary>
        /// Add a new type of measurement
        /// </summary>
        /// <param name="token">User token</param>
        /// <param name="measurementDefinition">Custom Definition</param>
        /// <param name="userPlantId">PlantId</param>
        /// <returns></returns>
        [HttpPut]        
        [Route("{userPlantId}")]
        public ObjectResult AddMeasurementDefinition([FromHeader] string token,[FromBody] CustomMeasurementDefinition measurementDefinition, int userPlantId)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            _dbContext.MeasurementDefinitions.Add(measurementDefinition);
            _dbContext.SaveChanges();
           return Ok("Measurement  definition added");
        }

        /// <summary>
        /// Method used to import data from a csv file about the plants
        /// </summary>
        /// <param name="csv">string containing the csv file</param>
        /// <param name="token">authentication token</param>
        /// <param name="userPlantId">plant id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userPlantId}")]
        public ObjectResult ImportData([FromBody] string csv, [FromHeader] string token, int userPlantId)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

           string[] csvRows = csv.Split("\n");
           string[] defintions = csvRows[0].Split(",");
           var md = new List<MeasurementDefinition>();
           var measurementsToAdd = new List<Measurement>();



            foreach (var definition in defintions)
            {
                MeasurementDefinition mdDb = new MeasurementDefinition();
                mdDb = _dbContext.CustomMeasurementDefinitions.FirstOrDefault(m => m.Name == definition && m.UserPlantId == userPlantId);
                if (mdDb == null)
                {
                    var plant = _dbContext.UserPlants.First(p => p.Id == userPlantId);
                    mdDb = _dbContext.MeasurementDefinitions.FirstOrDefault(m => m.Name == definition && m.PlantId == plant.PlantId);
                }
                md.Add(mdDb);
            }

            foreach ( var row in csvRows.Skip(1)) {
               string[] measurements = row.Split(",");
               for(int i = 0; i < measurements.Count(); i = i+2){
                   var m = new Measurement(){
                       MeasurementDefinitionId = md[i].Id,
                       UserPlantId = userPlantId,
                       Value = measurements[i],
                       Date = DateTime.Parse(measurements[i+1])                       
                   };  
                   measurementsToAdd.Add(m);          
               }
           }
           
           _dbContext.Measurements.AddRange(measurementsToAdd);
           _dbContext.SaveChanges();
           return Ok("Data is saved properly");
        }
    }
}