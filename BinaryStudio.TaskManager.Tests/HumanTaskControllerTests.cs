namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class HumanTaskControllerTests
    {
        private Mock<ITaskProcessor> taskProcessorMock;
        private Mock<IEmployeeRepository> employeeRepository;
        private HumanTasksController controller;
        private Mock<IUserProcessor> userProcessorMock;

        [SetUp]
        public void Init()
        {
            this.taskProcessorMock = new Mock<ITaskProcessor>();
            this.employeeRepository = new Mock<IEmployeeRepository>();
            this.userProcessorMock = new Mock<IUserProcessor>();
            this.controller = new HumanTasksController(
                this.taskProcessorMock.Object,
                this.employeeRepository.Object,
                this.userProcessorMock.Object);
        }

        [Test]
        public void Should_CreateAssignedTask_WhenMethodCreateCalledWithAssignedId()
        {
            //act
            this.controller.Create(new HumanTask() { AssigneeId = 5 });

            //assert            
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 5)), Times.Once());
        }

        [Test]
        public void Should_GetAllTasksFromTaskProcessor_WhenControllerPrepareMainPage()
        {
            //act
            controller.AllTasks();

            //assert
            this.taskProcessorMock.Verify(x => x.GetAllTasks(), Times.Once());
        }

        [Test]
        public void Should_GetTaskFromTaskProcessor_WhenMustBeShownTaskDetails()
        {
            //arrange
            this.taskProcessorMock.Setup(x => x.GetTaskById(3)).Returns(new HumanTask { Id = 3 });

            //act
            controller.Details(3);

            //assert
            this.taskProcessorMock.Verify(x => x.GetTaskById(3), Times.Once());
        }
    }
}