
namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class UserProcessorTests
    {
        private Mock<IUserRepository> userRepositoryMock;

        private CryptoProvider cryptoProvider;


        [SetUp]
        private void Setup()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.cryptoProvider = new CryptoProvider();
        }

        [Test]
        public void ShouldNot_CreateNewUser_WhenThisUserAlreadyExist()
        {            
            var userProcessor = new UserProcessor(this.userRepositoryMock.Object, this.cryptoProvider);
            const string Username = "username";
            const string Password = "password";
            const string Email = "email@domain.dom";
            const int Id = 100500;
            
            var user = new User
                {
                    Id = Id,
                    UserName = Username,
                    Email = Email                    
                };
            this.userRepositoryMock.Setup(x => x.GetByName(Username)).Returns(user);

            // act
            var result = userProcessor.CreateUser(Username, Password, Email, string.Empty);

            // assert
            Assert.AreEqual(result, false);            
        }

        [Test]
        public void Should_CreateNewUser_WhenThisUserNotExistingYet()
        {
            var userProcessor = new UserProcessor(this.userRepositoryMock.Object, this.cryptoProvider);
            const string Username = "username";
            const string Password = "password";
            const string Email = "email@domain.dom";                        
            this.userRepositoryMock.Setup(x => x.GetByName(Username)).Returns((User)null);

            // act
            var result = userProcessor.CreateUser(Username, Password, Email, string.Empty);

            // assert
            Assert.AreEqual(result, true);            
        }        
    }
}
