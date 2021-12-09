using System.Collections.Generic;
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
        /// Get all dashboards belonging to a user
        /// </summary>
        /// <param name="token">User token</param>
        /// <returns>A list of the dashboards and its info</returns>
        [HttpGet]        
        [Route("all")]
        public ObjectResult GetAllMyDashboards([FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            return Ok( _dbContext.Dashboards
                    .Include(d => d.Boards)
                    .Include(d => d.UserPlants)            
                    .AsNoTracking()
                    .AsParallel()
                    .Where(d => d.UserId == user.Id).ToList());
        }

        /// <summary>
        /// Creates a dashboard
        /// </summary>
        /// <param name="token">User authentication token</param>
        /// <param name="dahsboard">Dashboard object (Include UserPlants)</param>
        /// <returns>response message</returns>
        [HttpPost]
        public ObjectResult CreateDashboard([FromHeader] string token,[FromBody] Dashboard dahsboard){
            _dbContext.Dashboards.Add(dahsboard);
            _dbContext.SaveChanges();

            return Ok("Saved successfully");            
        }

        /// <summary>
        /// Delete an existing dashboard.
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="id">Dashboard id</param>
        /// <returns>Response message</returns>
        [HttpDelete]
        public ObjectResult DeleteDashboard([FromHeader] string token,
                                            int id)
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
        /// Get a dashboard by its id
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="id">Dashboard id</param>
        /// <returns>The dashboard</returns>
        [HttpGet]
        public ObjectResult GetDashboard([FromHeader] string token,
                                            int id)
        {
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            var dashboard =  _dbContext.Dashboards
            .Include(dash => dash.Boards)
            .Include(dash => dash.UserPlants)
            .FirstOrDefault(dash => dash.Id == id && dash.UserId == user.Id);

            return Ok(dashboard);            
        }

        /// <summary>
        /// Adds a board to a dashboard
        /// </summary>
        /// <param name="board">Board object with dashboard id and plant id (necessary)</param>
        /// <param name="token">User authenticaton token</param>
        /// <returns>Response message</returns>
        [HttpPost]
        [Route("board")]
        public ObjectResult AddBoard([FromBody] Board board,[FromHeader] string token){
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            _dbContext.Boards.Add(board);
            _dbContext.SaveChanges();

            return Ok("Added successfully");
        }

        /// <summary>
        /// Delete the board from the dashboard
        /// </summary>
        /// <param name="board">Board object</param>
        /// <param name="token">User authentication token</param>
        /// <returns>Response message</returns>
        [HttpDelete]
        [Route("board")]
        public ObjectResult RemoveBoard([FromBody] Board board,[FromHeader] string token){
            ControllerUtilities.TokenVerification(token, _dbContext,out var user, out var isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            _dbContext.Boards.Remove(board);
            _dbContext.SaveChanges();

            return Ok("Added successfully");
        }

        /// <summary>
        /// Add plants to a dashboard
        /// </summary>
        /// <param name="userPlants">List of plants to add to the dashboard</param>
        /// <param name="token">User token authentication</param>
        /// <param name="id">Dashboard's Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("plants")]
        public ObjectResult AddPlants([FromBody] List<UserPlant> userPlants, [FromHeader] string token, int id){
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            var dashboard = _dbContext.Dashboards.Include(dash=> dash.UserPlants).FirstOrDefault(dash => dash.Id == id);

            foreach (UserPlant plant in userPlants){
                var userPLant =_dbContext.UserPlants.FirstOrDefault(p=> p.Id == plant.Id);
                dashboard.UserPlants.Add(userPLant);
            }            
            _dbContext.SaveChanges();
            return Ok("All plants added successfully");
        }

        /// <summary>
        /// Remove plant from a dashboard
        /// </summary>
        /// <param name="userPlant">User plant to remove from the dashboard</param>
        /// <param name="token">User token authentication</param>
        /// <param name="id">Dashboard's Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("plants")]
        public ObjectResult RemovePlant([FromBody] UserPlant userPlant, [FromHeader] string token, int id){
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            var dashboard = _dbContext.Dashboards.Include(dash=> dash.UserPlants).FirstOrDefault(dash => dash.Id == id);
            var userPlantToRemove =_dbContext.UserPlants.FirstOrDefault(p=> p.Id == userPlant.Id);
            dashboard.UserPlants.Remove(userPlantToRemove);            
            _dbContext.SaveChanges();

            return Ok("All plants removed successfully");
        }               
    }
}