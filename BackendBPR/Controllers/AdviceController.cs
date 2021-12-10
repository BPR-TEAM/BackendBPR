using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendBPR.Database;
using BackendBPR.Utils;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

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
        private readonly IMapper _mapper;
        private readonly OrangeBushContext _dbContext;

        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="mapper">The mapper to map advice to custom advice</param>
        /// <param name="db">The database context to query</param>
        public AdviceController(ILogger<AuthController> logger, IMapper mapper, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
            _mapper = mapper;
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

        /// <summary>
        /// Get all the advices for a plant
        /// </summary>
        /// <param name="plantId">Plant's Id</param>
        /// <param name="token">User token</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomAdvice>),200)]
        public ActionResult GetAdvice(int plantId,[FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            var userAdvice = new List<CustomAdvice>();
            userAdvice = _dbContext.Advices
            .Include(advice => advice.UserAdvices)
            .Include(advice => advice.Tag)
              .ThenInclude( tag => tag.Plants)
            .Where( a => a.Tag.Plants.Any(a => a.Id == plantId))            
            .AsNoTracking()
            .AsParallel()
            .Select(a => {
                var customAdvice = _mapper.Map<CustomAdvice>(a);
                customAdvice.CurrentUserRole = a.UserAdvices.FirstOrDefault(userAdvice => userAdvice.UserId == user.Id).Type;
                return customAdvice;
            })
            .ToList();

            return Ok(userAdvice);
        }

        /// <summary>
        /// Like or dislike a device
        /// </summary>
        /// <param name="adviceId">Advice's Id</param>
        /// <param name="vote">Like -> 1 ||| Dislike -> 0</param>
        /// <param name="token">User token</param>
        /// <returns></returns>
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
                    AdviceId = adviceId,
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
        
        /// <summary>
        /// Give advice on a group of plants
        /// </summary>
        /// <param name="plantId">Plant you're commenting on</param>
        /// <param name="advice">The advice object</param>
        /// <param name="token">User token</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Give(int plantId, [FromBody] Advice advice ,[FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            _dbContext.Advices.Add(advice);
            _dbContext.SaveChanges();

            advice = _dbContext.Advices.LastOrDefault();

            _dbContext.UserAdvices.Add(new UserAdvice {
                UserId = user.Id,
                AdviceId = advice.Id,
                Type = AdviceRole.Creator
            });
            _dbContext.SaveChanges();
            
            return Ok("Advice added");
        }

        /// <summary>
        /// Dele your own comment
        /// </summary>
        /// <param name="adviceId">The advice id to delete</param>
        /// <param name="token">User token</param>
        /// <returns></returns>
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