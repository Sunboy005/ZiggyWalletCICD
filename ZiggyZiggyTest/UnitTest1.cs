using NUnit.Framework;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidLoginTest(string email, string password)
        {
            //Arranged
            var emailTolog = email;
            var passwordToLog = password;
            var expected=IAuthService.Login()

            //Actual


            //Assert


            //Assert.True(true);
        }
    }
}