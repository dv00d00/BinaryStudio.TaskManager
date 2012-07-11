using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;

using Moq;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Web.Controllers;

    [TestFixture]
    class AdminControllerTests
    {
        public AdminController controller;
        
        public Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Initialize()
        {            
            userRepositoryMock = new Mock<IUserRepository>();
            controller = new AdminController(userRepositoryMock.Object);
        }

        //[Test]
        //public void Should_ShowEmployDetails_WhenOpenedDetailsView()
        //{
        //    //arrange
        //    employeeRepositoryMock.Setup(x => x.GetById(1));

        //    //act             
        //    employeeRepositoryMock.DetailsEmployee(1);

        //    //assert
        //    employeeRepositoryMock.Verify(x => x.GetById(1), Times.Once());
        //}

    }
}
