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
using BackendBPR.ApiModels;
using AutoMapper;

namespace BackendBPR.Tests.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<OrangeBushContext> _dbMock;
        private readonly Mock<DbSet<User>> _dbSetMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;

        public AuthControllerTest()
        {
            _dbMock = new Mock<OrangeBushContext>();
            _dbSetMock = new Mock<DbSet<User>>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _mapperMock = new Mock<IMapper>();

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
            var controller = new AuthController(_loggerMock.Object,_mapperMock.Object,_dbMock.Object);
            _dbMock.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            _dbMock.Setup(t => t.SaveChanges());
            _mapperMock.Setup(t => t.Map<User>(It.IsAny<object>())).Returns(new User()
            {
                Birthday = new DateTime(),
                FirstName = "Habibi",
                Username = "Habibi420",
                Email = "habibi@habibiairlines.com",
                PasswordHash = "hashmebabyonemoretime"
            });

            var result = (OkObjectResult) controller.Register(new RegisterUserApi());

            if (result.StatusCode != null) 
                Assert.Equal(HttpStatusCode.OK, (HttpStatusCode) result.StatusCode);
            Assert.NotNull((string)result.Value);
            _dbMock.Verify(t=>t.SaveChanges(),Times.Once);
        }
    }
}