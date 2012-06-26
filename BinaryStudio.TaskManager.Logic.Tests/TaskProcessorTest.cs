namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    using NUnit.Framework;

    [TestFixture]
    public class TaskProcessorTest
    {
        // mmmmmore facts
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

        }
        [Test]
        public void Should_ReturnListOfTasksOfManagerByHisId()
        {
            var processor = new TaskProcessor();

        }
        [Test]
        public void Should_ReturnListOfNotassignedTasks()
        {

        }
    }

    public class TaskProcessor
    {
        public IEnumerable<Task> GetTasksList(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> GetTasksList()
        {
            //returns NotAssignedTasks
            throw new NotImplementedException();
        }
    }
}
