using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class HumanTaskControllerTests
    {
        private Mock<IHumanTaskRepository> mock;
        private Mock<ITaskProcessor> taskProcesorMock;
        private Mock<IEmployeeRepository> employeeRepository;

        private HumanTasksController controller;

        [SetUp]
        public void Init()
        {
            this.mock = new Mock<IHumanTaskRepository>();
            this.taskProcesorMock = new Mock<ITaskProcessor>();
            this.employeeRepository = new Mock<IEmployeeRepository>();
            
            this.controller = new HumanTasksController(mock.Object, taskProcesorMock.Object, this.employeeRepository.Object);
        }

        [Test]
        public void Should_MoveTask()
        {
            // Act

            this.controller.MoveTask(123, 123, 123);

            // Assert           

            this.taskProcesorMock.Verify(it => it.MoveTask(123, 123));
        }

        [Test]
        public void Should_TakeDataFromTaskProcessorAndEmployeeRepository_WhenLoadingManagerDetails()
        {
            // Act

            this.controller.ManagerDetails(12);

            // Assert           

            this.taskProcesorMock.Verify(it => it.GetTasksList(12));
            this.employeeRepository.Verify(it => it.GetById(12));
        } 

        [Test]
        public void Should_CreateTask_WhenCallCreate()
        {
            //arrange
            HumanTask task = new HumanTask();
                
            //act
            this.controller.Create(task);

            //assert
            this.taskProcesorMock.Verify(it => it.CreateTask(task),Times.Once());
        }
    }
}