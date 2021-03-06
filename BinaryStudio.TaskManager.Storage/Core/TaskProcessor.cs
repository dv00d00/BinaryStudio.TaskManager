// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskProcessor.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TaskProcessor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The task processor.
    /// </summary>
    public class TaskProcessor : ITaskProcessor
    {
        /// <summary>
        /// The human task repository.
        /// </summary>
        private readonly IHumanTaskRepository humanTaskRepository;

        /// <summary>
        /// The reminder repository.
        /// </summary>
        private readonly IReminderProcessor reminderProcessor;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProcessor"/> class.
        /// </summary>
        /// <param name="humanTaskRepository">
        /// The human task repository.
        /// </param>
        /// <param name="reminderProcessor">
        /// The reminder repository.
        /// </param>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        public TaskProcessor(IHumanTaskRepository humanTaskRepository, IReminderProcessor reminderProcessor, IUserRepository userRepository)
        {
            this.humanTaskRepository = humanTaskRepository;
            this.reminderProcessor = reminderProcessor;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// The create task without reminder.
        /// </summary>
        /// <param name="task">The current task.</param>
        public void CreateTask(HumanTask task)
        {
            string date = task.Finished.HasValue ? task.Finished.Value.ToShortDateString() : "";
            this.humanTaskRepository.Add(task);
            if (task.Finished.HasValue && task.AssigneeId.HasValue)
            {
                Reminder reminder = new Reminder
                {
                    ReminderDate = task.Finished.Value.AddDays(-1),
                    Content = "You need to do '" + task.Name + "' task for " + date ,
                    Task = task,
                    TaskId = task.Id,
                    UserId = task.AssigneeId.Value,
                    WasDelivered = false,
                    IsSend = true,
                    User = userRepository.GetById(task.AssigneeId.Value)
                };
                reminderProcessor.AddReminder(reminder);
            }
        }

        /// <summary>
        /// The create task with reminder.
        /// </summary>
        /// <param name="task">The current task.</param>
        /// <param name="reminder">The reminder.</param>
        public void CreateTask(HumanTask task, Reminder reminder)
        {
            this.humanTaskRepository.Add(task);

            // task.Id got its value from database insert
            var newTaskId = task.Id;

            reminder.TaskId = newTaskId;

            this.reminderProcessor.AddReminder(reminder);
        }

        /// <summary>
        /// The update task.
        /// </summary>
        /// <param name="task">The current task.</param>
        public void UpdateTask(HumanTask task)
        {
            this.humanTaskRepository.Update(task);
        }

        /// <summary>
        /// Adds the history.
        /// </summary>
        /// <param name="newHumanTask">The new human task.</param>
        public void AddHistory(HumanTaskHistory newHumanTask)
        {
            this.humanTaskRepository.AddHistory(newHumanTask);
        }

        /// <summary>
        /// The update task with reminder.
        /// </summary>
        /// <param name="task">The current task.</param>
        /// <param name="reminder">The reminder.</param>
        public void UpdateTask(HumanTask task, Reminder reminder)
        {
            this.humanTaskRepository.Update(task);

            this.reminderProcessor.UpdateReminder(reminder);
        }

        /// <summary>
        /// The delete current task with all reminders for this task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        public void DeleteTask(int taskId)
        {
           /* if (this.reminderProcessor.GetAll().Any(x => x.TaskId.HasValue && x.TaskId.Value == taskId))
            {
                this.reminderProcessor.DeleteRemindersForTask(taskId);
            }*/

            this.humanTaskRepository.Delete(taskId);
        }

        /// <summary>
        /// The move task between users.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        public void MoveTask(int taskId, int userId)
        {
            var taskToBeAssigned = this.humanTaskRepository.GetById(taskId);
            
            try
            {
                this.userRepository.GetById(userId);
                taskToBeAssigned.AssigneeId = userId;
            }
            catch
            {
                taskToBeAssigned.AssigneeId = null;
                
            }
            finally 
            {
                this.humanTaskRepository.Update(taskToBeAssigned);
            }
        }

        /// <summary>
        /// The move task to unassigned.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        public void MoveTaskToUnassigned(int taskId)
        {
            var task = this.humanTaskRepository.GetById(taskId);
            task.AssigneeId = null;
            this.humanTaskRepository.Update(task);
        }

        /// <summary>
        /// The close current task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        public void CloseTask(int taskId)
        {
            var taskToBeClosed = this.humanTaskRepository.GetById(taskId);
            var now = DateTime.Now;
            taskToBeClosed.Closed = now;
            this.humanTaskRepository.Update(taskToBeClosed);
            this.reminderProcessor.DeleteRemindersForTask(taskToBeClosed.Id);
        }

        /// <summary>
        /// The get tasks list.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetTasksList()
        {
            return this.humanTaskRepository.GetAll();
        }

        /// <summary>
        /// The get all tasks in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllTasksInProject(int projectId)
        {
            return this.humanTaskRepository.GetAllTasksInProject(projectId);
        }

        /// <summary>
        /// The get tasks list.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetTasksList(int userId)
        {
            return this.humanTaskRepository.GetAllTasksForEmployee(userId);
        }

        /// <summary>
        /// The get unassigned tasks.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetUnassignedTasks()
        {
            return this.humanTaskRepository.GetUnassingnedTasks();
        }

        /// <summary>
        /// The get task by id.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.HumanTask.
        /// </returns>
        public HumanTask GetTaskById(int taskId)
        {
            return this.humanTaskRepository.GetById(taskId);
        }

        /// <summary>
        /// The get all tasks.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllTasks()
        {
            return this.humanTaskRepository.GetAll();
        }

        /// <summary>
        /// The get priorities list.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; System.Web.Mvc.SelectListItem].
        /// </returns>
        public IEnumerable<SelectListItem> GetPrioritiesList()
        {
            IEnumerable<SelectListItem> priorities =
                this.humanTaskRepository.GetPriorities().ToList().Select(
                    it => new SelectListItem
                        {
                                     Text = it.Description,
                                     Value = it.Value.ToString(CultureInfo.InvariantCulture)
                                 });
            return priorities;
        }

        /// <summary>
        /// The get tasks in project list.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; System.Web.Mvc.SelectListItem].
        /// </returns>
        public IEnumerable<SelectListItem> GetOpenTasksListInProject(int projectId)
        {
            IEnumerable<SelectListItem> tasks =
                this.humanTaskRepository.GetAllTasksInProject(projectId).Where(x => x.Closed == null).Where(it => it.BlockingTaskId == 0).ToList().Select(
                    it => new SelectListItem
                        {
                    Text = it.Name,
                    Value = it.Id.ToString(CultureInfo.InvariantCulture)
                });
            return tasks;
        }

        /// <summary>
        /// The get un assigned tasks for project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetUnAssignedTasksForProject(int projectId)
        {
           return this.humanTaskRepository.GetUnassingnedTasks(projectId);
        }

        /// <summary>
        /// The get all history for task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTaskHistory].
        /// </returns>
        public IList<HumanTaskHistory> GetAllHistoryForTask(int taskId)
        {
            return this.humanTaskRepository.GetAllHistoryForTask(taskId);
        }

        /// <summary>
        /// The get all tasks for user in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllTasksForUserInProject(int projectId, int userId)
        {
            return this.humanTaskRepository.GetAllTasksForUserInProject(projectId, userId);
        }

        /// <summary>
        /// The get all history for user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTaskHistory].
        /// </returns>
        public IList<HumanTaskHistory> GetAllHistoryForUser(int userId)
        {
            return this.humanTaskRepository.GetAllHistoryForUser(userId);
        }

        /// <summary>
        /// The get all closed tasks for project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllClosedTasksForProject(int projectId)
        {
            var tasks = this.humanTaskRepository.GetAllTasksInProject(projectId);
            var openTasks = tasks.Where(x => x.Closed != (DateTime?)null);
            return openTasks;
        }

        /// <summary>
        /// The get all closed tasks for user in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllClosedTasksForUserInProject(int projectId, int userId)
        {
            var tasks = this.humanTaskRepository.GetAllTasksForUserInProject(projectId, userId);
            return tasks.Where(x => x.Closed != (DateTime?)null);
        }

        /// <summary>
        /// The get all open tasks for project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllOpenTasksForProject(int projectId)
        {
            var tasks = this.humanTaskRepository.GetAllTasksInProject(projectId);
            var openTasks = tasks.Where(x => x.Closed == (DateTime?)null);
            return openTasks;
        }

        /// <summary>
        /// The get all open tasks for user in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAllOpenTasksForUserInProject(int projectId, int userId)
        {
            var tasks = this.humanTaskRepository.GetAllTasksForUserInProject(projectId, userId);
            return tasks.Where(x => x.Closed == (DateTime?)null);
        }
    }
}