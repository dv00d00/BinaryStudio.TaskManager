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
            //throw new NotImplementedException();
           
            humanTaskRepository.Update(task);
            
            reminderRepository.Update(reminder);
        }

        public void DeleteTask(int taskId)
        {
            //throw new NotImplementedException();
            
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
            //returns AllTasks
            //throw new NotImplementedException();

            return humanTaskRepository.GetAll();
        }

        public IEnumerable<HumanTask> GetTasksList(int employeeId)
        {
            //throw new NotImplementedException();

            return humanTaskRepository.GetAllTasksForEmployee(employeeId);
        }

        public IEnumerable<HumanTask> GetUnassignedTasks()
        {
            //returns UnassignedTasks
            //throw new NotImplementedException();

            foreach (var task in humanTaskRepository.GetAll())
            {
                if (task.AssigneeId.HasValue == false)
                {
                    yield return task;
                }
            }
        }

        public HumanTask GetTaskById(int taskId)
        {
            return humanTaskRepository.GetById(taskId);
        }

        public void AssignTask(int taskId, int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}