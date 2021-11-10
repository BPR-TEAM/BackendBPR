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
        /// <param name="plantId">The plantId of the plant in question</param>
        /// <returns>A list of the advice for the given plant</returns>
        [HttpGet]
        [Route("default")]
        public ActionResult GetDefaultAdviceForPlant(int plantId)
        {
            List<Advice> defaultAdvice = new List<Advice>();
            defaultAdvice = (List<Advice>) _dbContext.Advices.Where(advice => advice.TagId == null).Include(advice => advice.Tag)
            .Select(a => a.Tag).Include(tag => tag.Plants).Where(plant => plant.Id == plantId);
            return Ok(defaultAdvice);
        }


        [HttpGet]
        public ActionResult GetAdvice(int plantId, [FromHeader] string _token)
        {
            if(!ControllerUtilities.TokenVerification(_token, _dbContext))
                return Unauthorized("User/token mismatch");

            List<UserAdvice> userAdvice = new List<UserAdvice>();
            userAdvice = (List<UserAdvice>) _dbContext.UserAdvices.Where(a => a.)
            return Ok(userAdvice);
        }

        [HttpPut]
        public ActionResult Vote(int adviceId, AdviceRole vote, [FromHeader] string _token)
        {
            if(!ControllerUtilities.TokenVerification(_token, _dbContext))
                return Unauthorized("User/token mismatch");
            return Unauthorized();
        }

        [HttpPost]
        public ActionResult Give(int plantId, [FromBody] Advice advice ,[FromHeader] string _token)
        {
            if(!ControllerUtilities.TokenVerification(_token, _dbContext))
                return Unauthorized("User/token mismatch");
            return Unauthorized();
        }

        [HttpDelete]
        public ActionResult DeleteOwn(int adviceId, [FromHeader] string _token)
        {
            if(!ControllerUtilities.TokenVerification(_token, _dbContext))
                return Unauthorized("User/token mismatch");
            return Unauthorized();
        }
    }
}