namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    public class TaskProcessor:ITaskProcessor
    {
        private readonly IHumanTaskRepository humanTaskRepository;

        public TaskProcessor(IHumanTaskRepository humanTaskRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
        }

        public void CreateTask(HumanTask task)
        {
            //throw new NotImplementedException();
            humanTaskRepository.Add(task);
        }

        public void CreateTask(HumanTask task,Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(HumanTask task)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(HumanTask task, Reminder reminder)
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


        public IEnumerable<HumanTask> GetTasksList()
        {
            //returns NotAssignedTasks
            throw new NotImplementedException();
        }

        public IEnumerable<HumanTask> GetTasksList(int employeeId)
        {
            throw new NotImplementedException();
        }

        public HumanTask GetTaskById(int taskId)
        {
            throw new NotImplementedException();
        }

        public void AssignTask(int taskId, int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}