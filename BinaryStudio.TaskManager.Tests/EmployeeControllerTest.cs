using BinaryStudio.TaskManager.Logic.Domain;
using Moq;
using NUnit.Framework;
using BinaryStudio.TaskManager.Web.Controllers;
using BinaryStudio.TaskManager.Logic.Core;

namespace BinaryStudio.TaskManager.Web.Tests
{
     [TestFixture]
    class EmployeeControllerTest
     {
         public EmployeeController employeeController;
         public Mock<IEmployeeRepository> employRepositoryMock;

         [SetUp]
         public void Initialize()
         {
            employRepositoryMock = new Mock<IEmployeeRepository>();
            employeeController = new EmployeeController(employRepositoryMock.Object);
         }

         [Test]
         public void Should_GetAllEmploies_WhenShowIndexListPage()
         {
             //arrange

             //act

             employeeController.Index();

             //assert
             employRepositoryMock.Verify(x => x.GetAll(),Times.Once());
         }

         [Test]
         public void Should_ShowEmployDetails_WhenOpenedDetailsView()
         {
             //arrange
             employRepositoryMock.Setup(x => x.GetById(1));

             //act             
             employeeController.Details(1);

             //assert
             employRepositoryMock.Verify(x => x.GetById(1),Times.Once());
         }

         [Test]
         public void Should_EditEmployeById()
         {
             //arrange

             //act
             employeeController.Edit(1);

             //assert
             employRepositoryMock.Verify(x => x.GetById(1), Times.Once());
         }

         [Test]
         public void Should_UpdateEmployAfterEdit_WhenModelStateIsValid()
         {
             //arrange
             Employee employee = new Employee();
             employRepositoryMock.Setup(x => x.Update(employee));

             //act
             employeeController.Edit(employee);

             //assert
             employRepositoryMock.Verify(x => x.Update(employee), Times.Once());
         }
         [Test]
         public void Should_DleteEmployById()
         {
             //arrange
             
             //act
             employeeController.DeleteConfirmed(1);

             //assert
             employRepositoryMock.Verify(x => x.Delete(1),Times.Once());

         }

    }
}
