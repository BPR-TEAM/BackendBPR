using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendBPR.Database;
using BackendBPR.Utils;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using FuzzySharp;

namespace BackendBPR.Controllers
{

    /// <summary>
    /// Controller for plants
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="db"></param>
        public PlantController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        /// <summary>
        /// Get a plant
        /// </summary>
        /// <param id="plantId"></param>
        /// <returns>The plant</returns>
        [HttpGet]
        public ObjectResult GetPlant(int id)
        {
           return Ok(_dbContext.Plants
                    .Include(a => a.Tags.Where( tags => tags.UserId == null))
                    .AsNoTracking()
                    .AsParallel()
                    .FirstOrDefault(a => a.Id == id));
        }

        
        /// <summary>
        /// Add tag to plant
        /// </summary>
        /// <param name="name">Name of plant</param>
        /// <param name="plantId">Plant id</param>
        /// <param name="token">User token</param>
        /// <returns>Message</returns>
        [HttpPost]
        [Route("tag")]
        public ObjectResult AddTag(string name, int plantId, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            var tag = _dbContext.Tags.FirstOrDefault( t => t.Name == name && t.Id == user.Id);
 
            if(tag == null){
                tag = new Tag {
                    Name = name,
                    UserId = user.Id
                };
            }

           _dbContext.Plants.Include(p => p.Tags).FirstOrDefault(p => p.Id == plantId).Tags.Add(tag);
           _dbContext.SaveChanges();
           return Ok("Tag added");
        }

        /// <summary>
        /// Removes tag from plant
        /// </summary>
        /// <param name = "name"></param>
        /// <param name="plantId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("tag")]
        public ObjectResult RemoveTag(string name, int plantId, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

           var tagToRemove =_dbContext.Tags.FirstOrDefault(t => t.Name == name && t.UserId == user.Id);
           _dbContext.Plants.Include(p => p.Tags).FirstOrDefault(p => p.Id == plantId).Tags.Remove(tagToRemove);
           _dbContext.SaveChanges();


           return Ok("Tag removed");
        }      

        /// <summary>
        /// Get a plant default tags
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns>The tags</returns>
        [HttpGet]
        [Route("tag")]
        public ObjectResult GetDefaultTags([FromQuery] int? plantId)
        {
           if(plantId == null){
               return Ok(_dbContext.Tags
                    .Where( p=> p.UserId == null));
           }
           return Ok(_dbContext.Tags
                    .Where( p => p.Id == plantId && p.UserId == null));
        }

        /// <summary>
        /// Search for plant by tags or it's name
        /// </summary>
        /// <param name="tags"></param>        
        /// <param name="name"></param>
        /// <returns>List of possible plants corresponding to the search</returns>
        [HttpPost]
        [Route("search")]
        public ObjectResult SearchPlant([FromBody] List<Tag> tags,string name)
        {
            var returnList = new List<string>();   
            if(tags.Any()){   
                var filteredList = new List<Plant>();                         
                foreach (Tag tag in tags){              
                    if(filteredList.Any()){
                        filteredList.Where(a => a.Tags.Contains(tag)).ToList();
                    }
                    filteredList = _dbContext.Plants
                    .Include(a =>  a.Tags.Where( tags => tags.UserId == null))
                    .AsNoTracking()
                    .AsParallel()
                    .Where(a => a.Tags.Any(t => t.Id == tag.Id))
                    .Select(p => new Plant() {Id = p.Id, Tags = p.Tags, CommonName = p.CommonName,ScientificName = p.ScientificName})
                    .ToList();
                }
                if(String.IsNullOrEmpty(name)){
                    returnList = filteredList.Select(plant => $"{plant.CommonName},{plant.ScientificName},{plant.Id}").ToList();
                    return Ok(returnList);
                }else{
                    var searchedList = SearchByPlantName(name, filteredList);
                    returnList = (searchedList.Any() ? searchedList : filteredList.Select(plant => $"{plant.CommonName},{plant.ScientificName},{plant.Id}").ToList());
                }
            }else{
                if(String.IsNullOrEmpty(name))
                  return NotFound("Text field is empty");
                var filteredList = SearchByPlantName(name);
                returnList = (filteredList.Any() ? filteredList : returnList);
            }
            return Ok(returnList); 
        }

        /// <summary>
        /// Method in charge of searching the plant
        /// </summary>
        /// <param name="searchText">Possible plant name</param>
        /// <param name="plants">Optional, in case the plant is to be searched in a list instead of the DB</param>
        /// <returns>A string list with Common and Scientific name and ID </returns>
         [NonAction]
        public List<string> SearchByPlantName(string searchText, List<Plant> plants = null)
        {
            if(plants == null)
                plants = _dbContext.Plants.Select(p => new Plant(){CommonName = p.CommonName, ScientificName = p.ScientificName, Id = p.Id})
                .AsNoTracking()
                .AsParallel()
                .ToList();
            
            var ratios = new Dictionary<string,int>();
            foreach (var plant in plants)
            {
                var commonNameRatio = (int)(Fuzz.Ratio(searchText, plant.CommonName));
                var scientificNameRatio = (int)(Fuzz.Ratio(searchText, plant.ScientificName));
                var ratio = commonNameRatio > scientificNameRatio ? commonNameRatio : scientificNameRatio;
              
                if (ratios.Count < 10)
                {
                    ratios.Add($"{plant.CommonName},{plant.ScientificName},{plant.Id}",ratio);
                }
                else
                {
                    var lowestRatio = ratios.Aggregate((l, r) => l.Value < r.Value ? l : r);
                    if (ratio > lowestRatio.Value)
                    {
                        ratios.Remove(lowestRatio.Key);
                        ratios.Add( $"{plant.CommonName},{plant.ScientificName},{plant.Id}",ratio);
                    }
                }            
            }       

            return ratios.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
        }

    }

}