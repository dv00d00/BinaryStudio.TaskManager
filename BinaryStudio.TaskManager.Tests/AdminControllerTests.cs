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
        public Mock<IEmployeeRepository> employeeRepositoryMock;
        public Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Initialize()
        {
            employeeRepositoryMock = new Mock<IEmployeeRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            controller = new AdminController(userRepositoryMock.Object, employeeRepositoryMock.Object);
        }

        [Test]
        public void Should_GetAllEmployees_WhenShowEmployeesListView()
        {
            //arrange

            //act

            controller.Index();

            //assert
            employeeRepositoryMock.Verify(x => x.GetAll(), Times.Once());
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


        [Test]
        public void Should_EditEmployeById()
        {
            //arrange

            //act
            controller.EditEmployee(1);

            //assert
            employeeRepositoryMock.Verify(x => x.GetById(1), Times.Once());
        }

        [Test]
        public void Should_UpdateEmployAfterEdit_WhenModelStateIsValid()
        {
            //arrange
            Employee employee = new Employee();
            employeeRepositoryMock.Setup(x => x.Update(employee));

            //act
            controller.EditEmployee(employee);

            //assert
            employeeRepositoryMock.Verify(x => x.Update(employee), Times.Once());
        }

        [Test]
        public void Should_DeleteEmployById()
        {
            //arrange

            //act
            controller.DeleteEmployeeConfirmed(1);

            //assert
            employeeRepositoryMock.Verify(x => x.Delete(1), Times.Once());
        }
    }
}
