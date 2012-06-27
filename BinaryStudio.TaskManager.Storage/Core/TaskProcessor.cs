namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    public class TaskProcessor
    {
        public IHumanTaskRepository htr { get; set; }
        public void CreateTask(Task task)
        {
            throw new NotImplementedException();
        }

        public void CreateTask(Task task,Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(int taskId,Task task)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(int taskId, Task task, Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public void MoveTask(int taskId, int employeeId)
        {
            throw new NotImplementedException();
        }

        public void MoveTaskToUnassigned(int taskId)
        {
            throw new NotImplementedException();
        }

        public void CloseTask(int taskId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Task> GetTasksList()
        {
            //returns NotAssignedTasks
            throw new NotImplementedException();
        }

        public IEnumerable<Task> GetTasksList(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}