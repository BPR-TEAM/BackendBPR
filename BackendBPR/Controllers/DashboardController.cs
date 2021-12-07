using System.Linq;
using System.Runtime.CompilerServices;
using BackendBPR.Database;
using BackendBPR.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly:InternalsVisibleTo("BackendBPR.Tests.Integration")]
namespace BackendBPR.Controllers
{
    /// <summary>
    /// Controller for dashboards
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly OrangeBushContext _dbContext;
        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="db">The database context to query</param>
        public DashboardController(ILogger<DashboardController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        /// <summary>
        /// Get all user plants, for a plant or just in general
        /// </summary>
        /// <param name="token">User token</param>
        /// <param name="plantId">(Optional) Only use this if you want all userPlant of a specific plant</param>
        /// <returns>A list of the user plants</returns>
        [HttpGet]        
        [Route("all")]
        public ObjectResult GetAllMyDashboards([FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            return Ok( _dbContext.Dashboards
                    .Include(d => d.Boards)
                    .AsNoTracking()
                    .AsParallel()
                    .Where(d => d.UserId == user.Id).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dahsboard"></param>
        /// <returns></returns>
        [HttpPost]
        public ObjectResult CreateDashboard([FromHeader] string token,[FromBody] Dashboard dahsboard){
            _dbContext.Dashboards.Add(dahsboard);
            _dbContext.SaveChanges();

            return Ok("Saved successfully");            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="id">Dashboard id</param>
        /// <returns></returns>
        [HttpDelete]
        public ObjectResult DeleteDashboard([FromHeader] string token,
                                            int? id)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            var toRemove =  _dbContext.Dashboards.FirstOrDefault(dash => dash.Id == id);
            _dbContext.Remove(toRemove);
            _dbContext.SaveChanges();

            return Ok("Removed successfully");            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="id">Dashboard id</param>
        /// <returns></returns>
        [HttpGet]
        public ObjectResult GetDashboard([FromHeader] string token,
                                            int? id)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            var dashboard =  _dbContext.Dashboards
            .Include(dash => dash.Boards)
            .FirstOrDefault(dash => dash.Id == id);

            return Ok(dashboard);            
        }

        [HttpPut]
        [Route("board")]
        public ObjectResult AddBoard([FromBody] Board board,[FromHeader] string token){
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            _dbContext.Boards.Add(board);
            _dbContext.SaveChanges();

            return Ok("Added successfully");
        }

        [Route("board")]
        [HttpPut]
        public ObjectResult RemoveBoard([FromBody] Board board,[FromHeader] string token){
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            _dbContext.Boards.Add(board);
            _dbContext.SaveChanges();

            return Ok("Added successfully");
        }

        



                    
    }
}