namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    public class TaskProcessor:ITaskProcessor
    {
        private readonly IHumanTaskRepository humanTaskRepository;
        private readonly IReminderRepository reminderRepository;

        public TaskProcessor(IHumanTaskRepository humanTaskRepository, IReminderRepository reminderRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.reminderRepository = reminderRepository;
        }

        public void CreateTask(HumanTask task)
        {
            humanTaskRepository.Add(task);
        }

        public void CreateTask(HumanTask task, Reminder reminder)
        {
            // throw new NotImplementedException();

            humanTaskRepository.Add(task);

            // task.Id got its value from database insert
            var newTaskId = task.Id;

            reminder.TaskId = newTaskId;

            reminderRepository.Add(reminder);

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