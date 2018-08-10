using System;
using System.Security.Claims;
using CallGate.Exceptions;
using CallGate.Repositories;
using CallGate.Services.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace UnitTests.Services.Helper
{
    public class AuthorizedUserHelperTest : IDisposable
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private IAuthorizedUserHelper _authorizedUserHelper;
        private Mock<IUserRepository> _userRepositoryMock;

        
        public AuthorizedUserHelperTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userRepositoryMock = new Mock<IUserRepository>();
            
            _authorizedUserHelper = new AuthorizedUserHelper(
                _httpContextAccessorMock.Object,
                _userRepositoryMock.Object
            );
        }
        
        public void Dispose()
        {
            _httpContextAccessorMock = null;
            _authorizedUserHelper = null;
        }
        
        [Theory]
        [InlineData("e0f4ae99-4e72-4563-bdfb-f46c3189b2f8")]
        [InlineData("fe5bfd7f-da87-458b-97e8-b17c90e7eb69")]
        [InlineData("9c6c647d-17d6-4987-85ce-e5228360ae86")]
        public void TestGetUserIdForLoggedUser(string expectedUserId)
        {
            // Arrange
            var claimMock = new Mock<Claim>("id", expectedUserId);
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>())).Returns(claimMock.Object);
            
            // Act
            var userId = _authorizedUserHelper.GetAuthorizedUserId();
           
            // Assert
            _httpContextAccessorMock.Verify(x => x.HttpContext.User.FindFirst(It.IsAny<string>()), Times.Once());
            userId.Should().NotBe(Guid.Empty);
            userId.ShouldBeEquivalentTo(new Guid(expectedUserId));
        }
        
        [Fact]
        public void TestGetUserIdForNotLoggedUser()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()));
            
            // Act
            Action act = () => _authorizedUserHelper.GetAuthorizedUserId();
            
            // Assert
            Assert.Throws<UserNotLoggedApiException>(act);
        }
    }
}