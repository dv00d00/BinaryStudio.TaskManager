
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
        [Test]
        public void ShouldNot_CreateNewUser_WhenThisUserAlreadyExist()
        {
            var userRepositoryMock = new Mock<IUserRepository>();

            var cryptoProvider = new CryptoProvider();

            var userProcessor = new UserProcessor(userRepositoryMock.Object, cryptoProvider);
            const string Username = "username";
            const string Password = "password";
            const string Email = "email@domain.dom";
            const string LinkedinId = "linked_in_id";
            const int Id = 100500;
            var salt = cryptoProvider.CreateSalt();
            var user = new User
                {
                    Id = Id,
                    UserName = Username,
                    Email = Email,
                    Credentials = new Credentials
                        {
                            Salt = salt,
                            Passwordhash = cryptoProvider.CreateCryptoPassword(Password, salt),
                            IsVerify = true
                        },
                    LinkedInId = LinkedinId
                };
            userRepositoryMock.Setup(x => x.GetByName(Username)).Returns(user);              

            // act
            userProcessor.CreateUser(Username, Password, Email, LinkedinId);

            // assert
            userRepositoryMock.Verify(it => it.CreateUser(It.Is<User>(x => x.Id == Id)), Times.Never());
        }
    }
}
