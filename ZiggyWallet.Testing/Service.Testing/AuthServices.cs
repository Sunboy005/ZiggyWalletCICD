using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZiggyZiggyWallet;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Implementations;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyWallet.Tests.Service.Testing
{
    public class AuthServiceTests : IClassFixture<TestFixture<Startup>>
    {
        private IAuthService Service { get; }
        // Arrange
        private const string Email = "tyty@123.com";
        private const string Password = "P@ssw0rd";
        private const bool RememberMe = true;
        public AuthServiceTests(TestFixture<Startup> fixture)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "abideklove@gmail.com",
                UserName = "minato"
            };
            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>() { "Admin", "Noob", "Elite" });
            fakeUserManager.Setup(x => x.FindByEmailAsync(Email)).ReturnsAsync(user);
            //fakeUserManager.Setup(x => x.(user, Password)).ReturnsAsync(true);
            var fakeSignInManager = new Mock<FakeSignInManager>();
            fakeSignInManager.Setup(
                    x => x.PasswordSignInAsync(user, Password, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            var jwtService = new Mock<IJWTServices>();
            //SERVICES CONFIGURATIONS
            Service = new AuthService(fakeUserManager.Object, fakeSignInManager.Object, jwtService.Object);
        }
        [Fact]
        public async Task ShouldLogin()
        {
            // Act
            var login = await Service.Login("tyty@123.com", Password, RememberMe);
            // Assert
            Assert.True(login.status);
        }
        [Theory]
        [InlineData("email", Password, RememberMe)]
        [InlineData(Email, "", RememberMe)]
        public async Task ShouldLoginFail(string email, string password, bool rememberMe)
        {
            // Act
            var login = await Service.Login(email, password, rememberMe);
            // Assert
            Assert.False(login.status);
        }
        //[Fact]
        //public async Task LoginEmailWrong()
        //{
        //    var login = await Service.Login("", Password, RememberMe);
        //    Assert.False(login.Status);
        //}
    }
}