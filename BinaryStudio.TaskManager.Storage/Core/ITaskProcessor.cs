namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The TaskProcessor interface.
    /// </summary>
    public interface ITaskProcessor
    {
        /// <summary>
        /// The create task without reminder.
        /// </summary>
        /// <param name="task">
        /// The current task.
        /// </param>
        void CreateTask(HumanTask task);

        /// <summary>
        /// The create task with reminder.
        /// </summary>
        /// <param name="task">
        /// The current task.
        /// </param>
        /// <param name="reminder">
        /// The reminder.
        /// </param>
        void CreateTask(HumanTask task, Reminder reminder);

        /// <summary>
        /// The update task.
        /// </summary>
        /// <param name="task">
        /// The current task.
        /// </param>
        void UpdateTask(HumanTask task);

        /// <summary>
        /// The update task with reminder.
        /// </summary>
        /// <param name="task">
        /// The current task.
        /// </param>
        /// <param name="reminder">
        /// The reminder.
        /// </param>
        void UpdateTask(HumanTask task, Reminder reminder);

        /// <summary>
        /// The add history.
        /// </summary>
        /// <param name="newHumanTaskHistory">
        /// The new human task history.
        /// </param>
        void AddHistory(HumanTaskHistory newHumanTaskHistory);

        /// <summary>
        /// The delete task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        void DeleteTask(int taskId);

        /// <summary>
        /// The move task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <param name="employeeId">
        /// The employee id.
        /// </param>
        void MoveTask(int taskId, int employeeId);

        /// <summary>
        /// The move task to unassigned.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        void MoveTaskToUnassigned(int taskId);

        /// <summary>
        /// The get all history for task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTaskHistory].
        /// </returns>
        IList<HumanTaskHistory> GetAllHistoryForTask(int taskId);

        /// <summary>
        /// The close task.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        void CloseTask(int taskId);

        /// <summary>
        /// The get task by id.
        /// </summary>
        /// <param name="taskId">
        /// The task id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.HumanTask.
        /// </returns>
        HumanTask GetTaskById(int taskId);

        /// <summary>
        /// The get unassigned tasks.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        IEnumerable<HumanTask> GetUnassignedTasks();

        /// <summary>
        /// The get tasks list.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        IEnumerable<HumanTask> GetTasksList(int userId);

        /// <summary>
        /// The get all tasks.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        IEnumerable<HumanTask> GetAllTasks();

        /// <summary>
        /// The get all tasks in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        IEnumerable<HumanTask> GetAllTasksInProject(int projectId);

        /// <summary>
        /// The get priorities list.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; System.Web.Mvc.SelectListItem].
        /// </returns>
        IEnumerable<SelectListItem> GetPrioritiesList();

        IEnumerable<HumanTask> GetUnAssignedTasksForProject(int projectId);

        IEnumerable<HumanTask> GetAllTasksForUserInProject(int projectId, int userId);

        IList<HumanTaskHistory> GetAllHistoryForUser(int userId);

        void AddNews(News news);

        IEnumerable<News> GetAllNewsForUser(int userId);
    }
}
