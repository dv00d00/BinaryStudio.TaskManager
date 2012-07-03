using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Tests
{
    using Logic.Core;
    using Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class HumanTaskControllerTests
    {
        private Mock<ITaskProcessor> taskProcesorMock;
        private Mock<IEmployeeRepository> employeeRepository;
        private HumanTasksController controller;

        [SetUp]
        public void Init()
        {
            this.taskProcesorMock = new Mock<ITaskProcessor>();
            this.employeeRepository = new Mock<IEmployeeRepository>();   
            this.controller = new HumanTasksController(taskProcesorMock.Object, this.employeeRepository.Object);
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
        public void Should_CreateAssignedTask_WhenMethodCreateCalledWithAssignedId()
        {
            //arrange            

            //act
            this.controller.Create(new HumanTask(){AssigneeId = 5, CreatorId = 5});

            //assert            
            this.taskProcesorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 5)), Times.Once());
            this.taskProcesorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.CreatorId == 5)), Times.Once());            
        }       
        
    }
}