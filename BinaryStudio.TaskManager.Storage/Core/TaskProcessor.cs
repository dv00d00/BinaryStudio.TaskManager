namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class TaskProcessor : ITaskProcessor
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
            this.humanTaskRepository.Add(task);
        }

        public void CreateTask(HumanTask task, Reminder reminder)
        {
            this.humanTaskRepository.Add(task);

            // task.Id got its value from database insert
            var newTaskId = task.Id;

            reminder.TaskId = newTaskId;

            this.reminderRepository.Add(reminder);
        }

        public void UpdateTask(HumanTask task)
        {
            this.humanTaskRepository.Update(task);
        }

        public void UpdateTask(HumanTask task, Reminder reminder)
        {
            this.humanTaskRepository.Update(task);
            
            this.reminderRepository.Update(reminder);
        }

        public void DeleteTask(int taskId)
        {
            foreach (var reminder in this.reminderRepository.GetAll())
            {
                if (reminder.TaskId == taskId)
                {
                    this.reminderRepository.Delete(reminder);
                }
            }

            this.humanTaskRepository.Delete(taskId);
        }

        public void MoveTask(int taskId, int employeeId)
        {
            var task = this.humanTaskRepository.GetById(taskId);
            task.AssigneeId = employeeId;
            this.humanTaskRepository.Update(task);

            foreach (var reminder in this.reminderRepository.GetAll())
            {
                if (reminder.TaskId == taskId)
                {
                    this.reminderRepository.Delete(reminder);
                }
            }
        }

        public void MoveTaskToUnassigned(int taskId)
        {
            var task = this.humanTaskRepository.GetById(taskId);
            task.AssigneeId = null;
            this.humanTaskRepository.Update(task);
        }

        public void CloseTask(int taskId)
        {
            var taskToBeClosed = this.humanTaskRepository.GetById(taskId);
            var now = DateTime.Now;
            taskToBeClosed.Closed = now;
            this.humanTaskRepository.Update(taskToBeClosed);
        }

        public IEnumerable<HumanTask> GetTasksList()
        {
            return this.humanTaskRepository.GetAll();
        }

        public IEnumerable<HumanTask> GetTasksList(int employeeId)
        {
            return this.humanTaskRepository.GetAllTasksForEmployee(employeeId);
        }

        public IEnumerable<HumanTask> GetUnassignedTasks()
        {
            return this.humanTaskRepository.GetUnassingnedTasks();
        }

        public HumanTask GetTaskById(int taskId)
        {
            return this.humanTaskRepository.GetById(taskId);
        }

        public void AssignTask(int taskId, int employeeId)
        {
            var taskToBeAssigned = this.humanTaskRepository.GetById(taskId);
            try
            {
                taskToBeAssigned.AssigneeId = employeeId;
                this.employeeRepository.GetById(employeeId);
                this.humanTaskRepository.Update(taskToBeAssigned);
            }
            catch
            {
                return;
            }
        }

        public IEnumerable<HumanTask> GetAllTasks()
        {
            return this.humanTaskRepository.GetAll();
        }
    }
}