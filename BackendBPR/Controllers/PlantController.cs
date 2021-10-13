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
    [ApiController]
    [Route("[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;

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
        public ObjectResult GetPlant([FromBody] int id)
        {
           return Ok(_dbContext.Plants
                    .Include(a => a.Tags)
                    .AsNoTracking()
                    .AsParallel()
                    .FirstOrDefault(a => a.Id == id));
        }

        /// <summary>
        /// Search for plant by tags or it's name
        /// </summary>
        /// <param tags="tags"></param>        
        /// <param name="name"></param>
        /// <returns>List of possible plants corresponding to the search</returns>
        [HttpGet]
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
                    .Include(a => a.Tags)
                    .AsNoTracking()
                    .AsParallel()
                    .Where(a => a.Tags.Contains(tag))
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

         [NonAction]
        public List<string> SearchByPlantName(string searchText, List<Plant> plants = null)
        {
            if(plants == null)
                plants = _dbContext.Plants.Select(p => new Plant(){CommonName = p.CommonName, Id = p.Id})
                .AsNoTracking()
                .AsParallel()
                .ToList();
            
            var ratios = new Dictionary<string,int>();
            foreach (var plant in plants)
            {
                var ratio = (int)(Fuzz.Ratio(searchText, plant.CommonName) * 0.5
                                  + Fuzz.PartialRatio(searchText,plant.CommonName) * 0.75
                                  + Fuzz.TokenSortRatio(searchText, plant.CommonName) + 0.75 )/2;
              
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
            return ratios.Keys.ToList();
        }

    }

}