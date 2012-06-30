using System.Linq;

namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    public class TaskProcessor: ITaskProcessor
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
            //throw new NotImplementedException();
           
            humanTaskRepository.Update(task);
        }

        public void UpdateTask(HumanTask task, Reminder reminder)
        {
           
            humanTaskRepository.Update(task);
            
            reminderRepository.Update(reminder);
        }

        public void DeleteTask(int taskId)
        {
            
            foreach (var reminder in reminderRepository.GetAll())
            {
                if (reminder.TaskId == taskId)
                {
                    reminderRepository.Delete(reminder);
                }
            }

            humanTaskRepository.Delete(taskId);
        }

        public void MoveTask(int taskId, int employeeId)
        {
            var task = humanTaskRepository.GetById(taskId);
            task.AssigneeId = employeeId;
            humanTaskRepository.Update(task);

        }

        public void MoveTaskToUnassigned(int taskId)
        {
            HumanTask task= humanTaskRepository.GetById(taskId);
            task.AssigneeId = null;
            humanTaskRepository.Update(task);

        }

        public void CloseTask(int taskId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<HumanTask> GetTasksList()
        {
            //returns AllTasks
            //throw new NotImplementedException();

            return humanTaskRepository.GetAll();
        }

        public IEnumerable<HumanTask> GetTasksList(int employeeId)
        {
            return humanTaskRepository.GetAllTasksForEmployee(employeeId);
        }

        public IEnumerable<HumanTask> GetUnassignedTasks()
        {
            //returns UnassignedTasks

            return humanTaskRepository.GetAll().Where(task => task.AssigneeId.HasValue == false);
        }

        public HumanTask GetTaskById(int taskId)
        {
            return humanTaskRepository.GetById(taskId);
        }

        public void AssignTask(int taskId, int employeeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HumanTask> GetAllTasks()
        {
            return humanTaskRepository.GetAll();
        }
    }
}