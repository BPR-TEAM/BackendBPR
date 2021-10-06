using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendBPR.Database;
using BackendBPR.Utils;


namespace BackendBPR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;

        public UserController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }
    }
    
}