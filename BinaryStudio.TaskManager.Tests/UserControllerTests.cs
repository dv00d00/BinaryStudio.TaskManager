using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Web.Content.Controllers;
using BinaryStudio.TaskManager.Web.Controllers;
using BinaryStudio.TaskManager.Web.Models;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Web.Tests
{   
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void Should_CreteAccount_WhenModelStateIsValid()
        {
            Mock<UserRepository> repositoryMock = new Mock<UserRepository>();                   
            UserController controller = new UserController(repositoryMock.Object);                   
            RegisterNewUserModel model = new RegisterNewUserModel
                                                {
                                                    userId = 1,
                                                    UserName = "Vasya", 
                                                    Email = "vasya@pupkin.com",
                                                    Password = "password",
                                                    ConfirmPassword = "password"
                                                };
            //act
            controller.Register(model);

            //assert
            repositoryMock.Verify(it => it.RegisterNewUser(It.Is<RegisterNewUserModel>(x=>x.userId == 1)), Times.Once());

        }

        [Test]
        public void ShouldNot_CreteAccount_WhenModelStateIsInvalid()
        {
            Mock<UserRepository> repositoryMock = new Mock<UserRepository>();
            UserController controller = new UserController(repositoryMock.Object);
            RegisterNewUserModel model = new RegisterNewUserModel
                                            {
                                                userId = 1,
                                                UserName = "Vasya",
                                                Email = "vasya@pupkin.com",
                                                Password = "password",
                                                ConfirmPassword = "password"
                                            };
            controller.ModelState.AddModelError("Error", "ErrorMessage");

            //act
            controller.Register(model);

            //assert
            repositoryMock.Verify(it => it.RegisterNewUser(It.Is<RegisterNewUserModel>(x => x.userId == 1)), Times.Never());

        }

        [Test]
        public void Should_DeleteUser()
        {
        }
    }
}
