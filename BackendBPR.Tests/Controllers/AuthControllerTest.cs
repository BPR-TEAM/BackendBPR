using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using BackendBPR.Controllers;
using BackendBPR.Database;
using Xunit;

namespace BackendBPR.Tests.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<OrangeBushContext> _dbMock;
        private readonly Mock<DbSet<User>> _dbSetMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;

        public AuthControllerTest()
        {
            _dbMock = new Mock<OrangeBushContext>();
            _dbSetMock = new Mock<DbSet<User>>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            var userToBeAuthenticated = new User()
            {
                Id = 0,
                Birthday = new DateTime(2000,4,3),
                Country = "string",
                Email = "string",
                FirstName = "string",
                PasswordHash = "string",
                Username = "stringy",
                Token = "0=asdhfhgfd",
                PasswordSalt = new byte[0]
            };
            
            var data = new List<User>
            {
                userToBeAuthenticated
            }.AsQueryable();
            
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        [Fact]
        public void RegisterTest()
        {
            var controller = new AuthController(_loggerMock.Object,_dbMock.Object);
            _dbMock.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMock.Setup(t => t.SaveChanges());

            User testUser = new User()
            {
                Birthday = new DateTime(),
                FirstName = "Habibi",
                Username = "Habibi420",
                Email = "habibi@habibiairlines.com",
                PasswordHash = "hashmebabyonemoretime"
            };

            var result = (OkObjectResult) controller.Register(testUser);

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.NotNull((string)result.Value);
            _dbMock.Verify(t=>t.SaveChanges(),Times.Once);
        }
    }
}