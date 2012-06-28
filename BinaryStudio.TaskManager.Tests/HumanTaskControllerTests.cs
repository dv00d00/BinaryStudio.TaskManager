namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class HumanTaskControllerTests
    {
        [Test]
        public void ShouldMoveTask()
        {
            // Arrange

            var mock = new Mock<IHumanTaskRepository>();
            var taskProcesorMock = new Mock<ITaskProcessor>();
            var emplProcessorMOck = new Mock<IEmployeeRepository>();

            var controller = new HumanTasksController(mock.Object, taskProcesorMock.Object,emplProcessorMOck.Object);

            // Act

            controller.MoveTask(123, 123, 123);

            // Assert           

            taskProcesorMock.Verify(it => it.MoveTask(123, 123));
        } 
    }
}