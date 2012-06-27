namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class TaskProcessorTest
    {
        [Test]
        public void Should_AddTask()
        {

        }
        [Test]
        public void Should_AddTaskWithReminder()
        {

        }
        [Test]
        public void Should_AssignTask_WhenSuchEmployeExists()
        {

        }
        [Test]
        public void ShouldNot_AssignTask_WhenSuchEmployeeDoesNotExist()
        {

        }
        [Test]
        public void Should_UpdateTask_WhenManagerIsTrying()
        {

        }
        [Test]
        public void ShouldNot_UpdateTask_WhenItIsAlreadyDone()
        {

        }
        [Test]
        public void Should_DeleteTask()
        {
            var humanTaskRepository = new Mock<IHumanTaskRepository>();
            var processor = new TaskProcessor() { htr = humanTaskRepository.Object };
        }
        [Test]
        public void Should_ReturnListOfTasksOfEmployeeByHisId()
        {

        }
        [Test]
        public void Should_ReturnListOfNotassignedTasks()
        {
            var processor = new TaskProcessor();
            
        }
    }
}
