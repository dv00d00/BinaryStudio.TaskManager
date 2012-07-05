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

        [SetUp]
        public void Init()
        {
            this.taskProcessorMock = new Mock<ITaskProcessor>();
            this.employeeRepository = new Mock<IEmployeeRepository>();
            this.controller = new HumanTasksController(taskProcessorMock.Object, this.employeeRepository.Object);
        }
        
        [Test]
        public void Should_CreateAssignedTask_WhenMethodCreateCalledWithAssignedId()
        {
            //arrange            

            //act
            this.controller.Create(new HumanTask(){AssigneeId = 5});

            //assert            
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 5)), Times.Once());            
        }       

        [Test]
        public void Should_GetAllTasksFromTaskProcessor_WhenControllerPrepareMainPage()
        {
            //act
            controller.Index();

            //assert
            this.taskProcessorMock.Verify(x => x.GetAllTasks(), Times.Once());
        }

        [Test]
        public void Should_GetTaskFromTaskProcessor_WhenMustBeShownTaskDetails()
        {
            //act
            controller.Details(3);

            //assert
            this.taskProcessorMock.Verify(x=>x.GetTaskById(3), Times.Once());
        }
    }
}