using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using BackendBPR.ApiModels;
using BackendBPR.Database;
using BackendBPR.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("BackendBPR.Tests.Integration")]
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
        private readonly IMapper _mapper;
        private readonly OrangeBushContext _dbContext;
        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="mapper">The mapper to use</param>
        /// <param name="db">The database context to query</param>
        public DashboardController(ILogger<DashboardController> logger, IMapper mapper, OrangeBushContext db)
        {
            _logger = logger;
            _mapper = mapper;
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
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if (!isVerified)
                return Unauthorized("User/token mismatch");

            return Ok(_dbContext.Dashboards
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
        public ObjectResult CreateDashboard([FromHeader] string token, [FromBody] CreateDashboardApi dahsboard)
        {
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if (!isVerified)
                return Unauthorized("User/token mismatch");

            var dashboardDb = _mapper.Map<Dashboard>(dahsboard);
            dashboardDb.UserId = user.Id;

            foreach (UserPlant plant in dahsboard.UserPlants)
            {
                try
                {
                    var userPLant = _dbContext.UserPlants.FirstOrDefault(p => p.Id == plant.Id);
                    dashboardDb.UserPlants.Add(userPLant);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            _dbContext.Dashboards.Add(dashboardDb);
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
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if (!isVerified)
                return Unauthorized("User/token mismatch");

            var toRemove = _dbContext.Dashboards.FirstOrDefault(dash => dash.Id == id);
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
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if (!isVerified)
                return Unauthorized("User/token mismatch");

            var dashboardDb = _dbContext.Dashboards
            .Include(dash => dash.UserPlants)
            .AsNoTracking()
            .AsParallel()
            .FirstOrDefault(dash => dash.Id == id && dash.UserId == user.Id);

            var dashboard = _mapper.Map<GetDashboardApi>(dashboardDb);
            dashboard.Boards = _dbContext.Boards
            .Where(b => b.DashboardId == dashboardDb.Id)
            .AsNoTracking()
            .ToList();

            dashboard.UserPlants = dashboardDb.UserPlants
            .Select(up =>
            {
                var plants = _dbContext.Plants.AsNoTracking().FirstOrDefault(p => p.Id == up.PlantId);
                var measurements = _dbContext
                .Measurements
                .Include(m => m.MeasurementDefinition)
                .Where(m => m.UserPlantId == up.Id).AsNoTracking().ToList();
                up.Plant = plants;
                up.Measurements = measurements;
                var mapped = _mapper.Map<UserPlantDashboardApi>(up);
                return mapped;
            }).ToList();

            return Ok(dashboard);
        }

        /// <summary>
        /// Adds a board to a dashboard
        /// </summary>
        /// <param name="board">Board object - Only is ID is unnecessary</param>
        /// <param name="token">User authenticaton token</param>
        /// <returns>Response message</returns>
        [HttpPost]
        [Route("board")]
        public ObjectResult AddBoard([FromBody] BoardApi board, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if (!isVerified)
                return Unauthorized("User/token mismatch");

            var boardDb = _mapper.Map<Board>(board);
            _dbContext.Boards.Add(boardDb);
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
        public ObjectResult RemoveBoard([FromBody] BoardApi board, [FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var isVerified);
            if (!isVerified)
                return Unauthorized("User/token mismatch");

            var boardDb = _mapper.Map<Board>(board);
            _dbContext.Boards.Remove(boardDb);
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
        public ObjectResult AddPlants([FromBody] List<UserPlant> userPlants, [FromHeader] string token, int id)
        {
            if (!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            var dashboard = _dbContext.Dashboards.Include(dash => dash.UserPlants).FirstOrDefault(dash => dash.Id == id);

            foreach (UserPlant plant in userPlants)
            {
                var userPLant = _dbContext.UserPlants.FirstOrDefault(p => p.Id == plant.Id);
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
        public ObjectResult RemovePlant([FromBody] UserPlantApi userPlant, [FromHeader] string token, int id)
        {
            if (!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            var dashboard = _dbContext.Dashboards
            .Include(dash => dash.UserPlants)
            .Include(dash => dash.Boards)
            .FirstOrDefault(dash => dash.Id == id);

            var userPlantToRemove = _dbContext.UserPlants.FirstOrDefault(p => p.Id == userPlant.Id);
            dashboard.UserPlants.Remove(userPlantToRemove);

            if (!dashboard.UserPlants.Any(p => p.PlantId == userPlantToRemove.PlantId))
            {
                var boardsToRemove = dashboard.Boards.Where(b => b.PlantId == userPlantToRemove.PlantId);
                foreach (Board board in boardsToRemove)
                    dashboard.Boards.Remove(board);
            }
            _dbContext.SaveChanges();

            return Ok("All plants removed successfully");
        }
    }
}