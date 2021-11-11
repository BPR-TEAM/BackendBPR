using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendBPR.Database;
using BackendBPR.Utils;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BackendBPR.Controllers
{
    /// <summary>
    /// Controller that is responsible for advice
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AdviceController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;

        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="db">The database context to query</param>
        public AdviceController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        /// <summary>
        /// Gets the general advice for a given plant
        /// </summary>
        /// <returns>A list of the general advice</returns>
        [HttpGet]
        [Route("default")]
        public ActionResult GetDefaultAdvice()
        {
            var defaultAdvice = new List<Advice>();
            defaultAdvice = _dbContext.Advices
            .Where(advice => advice.TagId == null)
            .AsNoTracking()
            .AsParallel()
            .ToList();

            return Ok(defaultAdvice);
        }


        [HttpGet]
        public ActionResult GetAdvice(int plantId,[FromHeader] string token)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            var userAdvice = new List<Advice>();
            userAdvice = _dbContext.Advices
            .Include(advice => advice.UserAdvices)
            .Include(advice => advice.Tag)
            .ThenInclude( tag => tag.Plants)
            .Where( a => a.Tag.Plants.Any(a => a.Id == plantId))
            .AsNoTracking()
            .AsParallel()
            .ToList();

            return Ok(userAdvice);
        }

        [HttpPut]
        public ActionResult Vote(int adviceId, AdviceRole vote, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");
            
            var message = "";
            var userAdvice = _dbContext.UserAdvices.FirstOrDefault(a => a.AdviceId == adviceId && a.UserId == user.Id);
            if(userAdvice == null){
                _dbContext.UserAdvices.Add(new UserAdvice(){
                    AdviceId =adviceId,
                    UserId = user.Id,
                    Type = vote
                });
                message = "UserAdvice added";
            } else if(userAdvice.Type != AdviceRole.Creator && userAdvice.Type != vote){
                userAdvice.Type = vote;                
                message = "UserAdvice type updated";
            } else if(userAdvice.Type != AdviceRole.Creator && userAdvice.Type == vote){
                _dbContext.UserAdvices.Remove(userAdvice);          
               message = "UserAdvice removed";
            }
            _dbContext.SaveChanges();
            return Ok(message);
        }

        [HttpPost]
        public ActionResult Give(int plantId, [FromBody] Advice advice ,[FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            _dbContext.Advices.Add(advice);
            _dbContext.UserAdvices.Add(new UserAdvice {
                UserId = user.Id,
                AdviceId = advice.Id,
                Type = AdviceRole.Creator
            });
            _dbContext.SaveChanges();
            
            return Ok("Advice added");
        }

        [HttpDelete]
        public ActionResult DeleteOwn(int adviceId, [FromHeader] string token)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            var advice = _dbContext.Advices.FirstOrDefault(a=>a.Id == adviceId);
            _dbContext.Advices.Remove(advice);
            _dbContext.SaveChanges();
            return Ok("Advice was removed");
        }
    }
}