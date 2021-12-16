using System.Linq;
using BackendBPR.Database;
using BackendBPR.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendBPR.Controllers
{
    /// <summary>
    /// Controller that is responsible for tags
    /// </summary>
    [ApiController]    
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly OrangeBushContext _dbContext;

        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="db">The database context to query</param>
        public TagController(ILogger<TagController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        

        /// <summary>
        /// Gets all the tags (user and default)
        /// </summary>
        /// <param name="plantId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/MyPlant/[controller]")]
        public ObjectResult GetPlantTags([FromQuery] int plantId, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

           return Ok(_dbContext.Tags
                    .Where( p => p.Id == plantId && (p.UserId == user.Id || p.UserId == null)));
        }

        /// <summary>
        /// Gets all the tags (user)
        /// </summary>
        /// <param name="plantId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public ObjectResult GetTags([FromQuery] int plantId, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

           return Ok(_dbContext.Tags
                    .Where(p => p.UserId == user.Id));
        }


         /// <summary>
        /// Deletes tag entirely from the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete]
        public ObjectResult DeleteTag(string name, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

           var tagToRemove =_dbContext.Tags.FirstOrDefault(t => t.Name == name && t.UserId == user.Id);
           _dbContext.Tags.Remove(tagToRemove);
           _dbContext.SaveChanges();


           return Ok("Tag deleted");
        }





        
    }
}