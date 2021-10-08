using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using BackendBPR.Database;
using BackendBPR.Utils;
using Xunit;

namespace BackendBPR.Tests.Utilities
{
    public class ControllerUtilitiesTest
    {
        private readonly Mock<OrangeBushContext> _dbMOck;
        private readonly Mock<DbSet<User>> _dbSetMock;
        

        public ControllerUtilitiesTest()
        {
            _dbMOck = new Mock<OrangeBushContext>();
            _dbSetMock = new Mock<DbSet<User>>();
            var data = new List<User>
            {
                new User() { Id = 1,
                    Birthday =  new System.DateTime(),
                    Country =  "string",
                    Email =  "string",
                    FirstName =  "string",
                    PasswordHash =  "string",
                    Username =  "string",
                    Token = "ssss",
                    PasswordSalt = new byte[0]
                }
            }.AsQueryable();
            
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        [Fact]
        public void TokenVerification_MalformedToken()
        {
            Assert.False(ControllerUtilities.TokenVerification("5=sada=sdf", _dbMOck.Object));
        }
        
        [Theory]
        [InlineData("1=sssd")]
        [InlineData("2=ssss")]
        public void TokenVerification_UserIsNullOrTokenIsWrong(string token)
        {
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            Assert.False(ControllerUtilities.TokenVerification(token, _dbMOck.Object));
        }

        [Fact]
        public void TokenVerification_TokenVerifiedSuccessfully()
        {
            var token = "1=ssss";
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            Assert.True(ControllerUtilities.TokenVerification(token, _dbMOck.Object));
        }
        
        [Fact]
        public void TokenVerification_Out_MalformedToken()
        {
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            ControllerUtilities
                .TokenVerification("5=sada=sdf", _dbMOck.Object, out var user, out var isVerified);
            
            Assert.False(isVerified);
            Assert.Null(user);
        }
        
        [Theory]
        [InlineData("1=sssd")]
        [InlineData("2=ssss")]
        public void TokenVerification_Out_UserIsNullOrTokenIsWrong(string token)
        {
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            ControllerUtilities
                .TokenVerification(token, _dbMOck.Object, out var user, out var isVerified);
            
            Assert.False(isVerified);
            Assert.Null(user);
        }

        [Fact]
        public void TokenVerification_Out_TokenVerifiedSuccessfully()
        {
            _dbMOck.Setup(t=> t.Users).Returns(_dbSetMock.Object);
            ControllerUtilities
                .TokenVerification("1=ssss", _dbMOck.Object, out var user, out var isVerified);
            
            Assert.True(isVerified);
            Assert.NotNull(user);
        }
        
        
    }
}