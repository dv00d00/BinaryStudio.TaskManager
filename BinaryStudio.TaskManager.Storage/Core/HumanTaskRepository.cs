// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HumanTaskRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the HumanTaskRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The human task repository.
    /// </summary>
    public class HumanTaskRepository : IHumanTaskRepository
    {
        /// <summary>
        /// The data base context.
        /// </summary>
        private readonly DataBaseContext dataBaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HumanTaskRepository"/> class.
        /// </summary>
        /// <param name="dataBaseContext">
        /// The data base context.
        /// </param>
        public HumanTaskRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        /// <summary>
        /// The get for creator.
        /// </summary>
        /// <param name="creatorId">
        /// The creator id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetForCreator(int creatorId)
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.CreatorId == creatorId).ToList();
        }

        /// <summary>
        /// The get all tasks for employee.
        /// </summary>
        /// <param name="employeeId">
        /// The employee id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IList<HumanTask> GetAllTasksForEmployee(int employeeId)
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.AssigneeId == employeeId).ToList();
        }

        /// <summary>
        /// The get all tasks in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IList<HumanTask> GetAllTasksInProject(int projectId)
        {
            return this.dataBaseContext.Projects.First(it => it.Id == projectId).Tasks.ToList();
        }
                
        /// <summary>
        /// The get unassingned tasks.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IList<HumanTask> GetUnassingnedTasks()
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.AssigneeId.Equals(null)).ToList();
        }

        public IList<HumanTask> GetUnassingnedTasks(int projectId)
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.AssigneeId.Equals(null)&& it.ProjectId==projectId).ToList();
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IEnumerable<HumanTask> GetAll()
        {
            return this.dataBaseContext.HumanTasks.ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="humanTaskId">
        /// The human task id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.HumanTask.
        /// </returns>
        public HumanTask GetById(int humanTaskId)
        {
            return this.dataBaseContext.HumanTasks.Single(it => it.Id == humanTaskId);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="humanTaskId">
        /// The human task id.
        /// </param>
        public void Delete(int humanTaskId)
        {
            HumanTask humantask = this.dataBaseContext.HumanTasks.Single(x => x.Id == humanTaskId);
            this.dataBaseContext.Entry(humantask).State = EntityState.Deleted;
            this.dataBaseContext.SaveChanges();
        }
        
        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        public void Update(HumanTask humanTask)
        {
            var task = dataBaseContext.Entry(humanTask).State = EntityState.Modified;

            //this.dataBaseContext.Entry(humanTask).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
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
            return this.dataBaseContext.HumanTaskHistories.Where(x => x.Task.Id == taskId).ToList();
        }
        
        /// <summary>
        /// The add new human task.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.HumanTask.
        /// </returns>
        public void Add(HumanTask humanTask)
        {
            this.dataBaseContext.Entry(humanTask).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public void AddHistory(HumanTaskHistory humanTaskHistory)
        {
            this.dataBaseContext.Entry(humanTaskHistory).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public IQueryable<Priority> GetPriorities()
        {
            return this.dataBaseContext.Priorities;
        }

        public IEnumerable<HumanTask> GetAllTasksForUserInProject(int projectId, int userId)
        {
            return this.dataBaseContext.HumanTasks.Where(x => x.ProjectId == projectId && x.AssigneeId == userId);
        }

        public IList<HumanTaskHistory> GetAllHistoryForUser(int userId)
        {
            var projects = this.dataBaseContext.Users.First(x => x.Id == userId).UserProjects;
            var taskHistories = new List<HumanTaskHistory>();
            foreach (var project in projects )
            {
                foreach (var task in project.Tasks)
                {
                    taskHistories.AddRange(GetAllHistoryForTask(task.Id));
                }
            }
            return taskHistories;
        }


    }
}