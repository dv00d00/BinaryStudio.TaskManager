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
        private readonly IEmployeeRepository employeeRepository;

        public TaskProcessor(IHumanTaskRepository humanTaskRepository, IReminderRepository reminderRepository, IEmployeeRepository employeeRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.reminderRepository = reminderRepository;
            this.employeeRepository = employeeRepository;
        }

        public void CreateTask(HumanTask task)
        {
            humanTaskRepository.Add(task);
        }

        public void CreateTask(HumanTask task, Reminder reminder)
        {

            humanTaskRepository.Add(task);

            // task.Id got its value from database insert
            var newTaskId = task.Id;

            reminder.TaskId = newTaskId;

            reminderRepository.Add(reminder);

        }

        public void UpdateTask(HumanTask task)
        {
           
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

            foreach (var reminder in reminderRepository.GetAll())
            {
                if (reminder.TaskId == taskId)
                {
                    reminderRepository.Delete(reminder);
                }
            }

        }

        public void MoveTaskToUnassigned(int taskId)
        {
            var task= humanTaskRepository.GetById(taskId);
            task.AssigneeId = null;
            humanTaskRepository.Update(task);

        }

        public void CloseTask(int taskId)
        {
            var taskToBeClosed = humanTaskRepository.GetById(taskId);
            var now = DateTime.Now;
            taskToBeClosed.Closed = now;
            humanTaskRepository.Update(taskToBeClosed);
        }


        public IEnumerable<HumanTask> GetTasksList()
        {
            return humanTaskRepository.GetAll();
        }

        public IEnumerable<HumanTask> GetTasksList(int employeeId)
        {
            return humanTaskRepository.GetAllTasksForEmployee(employeeId);
        }

        public IEnumerable<HumanTask> GetUnassignedTasks()
        {
            //returns UnassignedTasks

            return humanTaskRepository.GetUnassingnedTasks();
        }

        public HumanTask GetTaskById(int taskId)
        {
            return humanTaskRepository.GetById(taskId);
        }

        public void AssignTask(int taskId, int employeeId)
        {
            var taskToBeAssigned = humanTaskRepository.GetById(taskId);
            try
            {
                taskToBeAssigned.AssigneeId = employeeId;
                employeeRepository.GetById(employeeId);
                humanTaskRepository.Update(taskToBeAssigned);
            }
            catch
            {
                return;
            }
        }

        public IEnumerable<HumanTask> GetAllTasks()
        {
            return humanTaskRepository.GetAll();
        }
    }
}