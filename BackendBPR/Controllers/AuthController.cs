using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendBPR.Database;
using BackendBPR.Utils;


[assembly:InternalsVisibleTo("BackendBPR.Tests.Integration")]
namespace BackendBPR.Controllers
{
    /// <summary>
    /// Controller that is responsible for authentication and authorization
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;
        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="db">The database context to query</param>
        public AuthController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        
        /// <summary>
        /// User can register here
        /// </summary>
        /// <param name="user">Should contain all the info up to country</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public ObjectResult Register([FromBody] User user)
        {
            var password = user.PasswordHash;
            
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            user.PasswordHash = HashPassword(salt, password);
            user.PasswordSalt =salt;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Ok("Registration complete!");
        }
        
        
        /// <summary>
        /// User can log in here
        /// </summary>
        /// <param name="user">Email and password must be filled</param>
        /// <returns>Authentication token </returns> 
        /// <response code="200">Login successful returns token</response>
        /// <response code="401">"Wrong credentials</response>     
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(string),200)]
        public ObjectResult Login([FromBody] User user)
        {
            if (String.IsNullOrEmpty(user.PasswordHash) || String.IsNullOrEmpty(user.Email))
            {
                return Unauthorized("Fill the fields");
            }
            
            var dbUser = _dbContext.Users
                .FirstOrDefault(a => a.Email == user.Email);

            if (dbUser == null)
            {
                return Unauthorized("User does not exist");
            }
            
            var salt = dbUser.PasswordSalt;
            var givenPassword = HashPassword(salt, user.PasswordHash);

            if (dbUser.PasswordHash == givenPassword)
            {
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=","");
                GuidString = GuidString.Replace("+","");
                dbUser.Token = GuidString;
                _dbContext.SaveChanges();
                return Ok(dbUser.Id+"="+dbUser.Token);
            }
            return Unauthorized("Wrong credentials");
        }
        
        /// <summary>
        /// Logs out the user
        /// </summary>
        /// <param name="token">Auth token should be sent to confirm user</param>
        /// <response code="200">Logout successful</response>
        /// <response code="400">"User or token do not exist</response>     
        [HttpPost]
        [Route("Logout")]
        public ObjectResult Logout([FromHeader] string token)
        {
            ControllerUtilities.TokenVerification(token, _dbContext, out var user, out var verified);
            if (verified)
            {
                user.Token = "";
                _dbContext.SaveChanges();
                return Ok("Logout successful");
            }
            else
            {
                return BadRequest("User or token do not exist");
            }
            
        }

        [NonAction]
        internal static string HashPassword(byte[] salt, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

    }
}