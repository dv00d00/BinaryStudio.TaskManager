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
        /// The get unassingned tasks.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IList`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.HumanTask].
        /// </returns>
        public IList<HumanTask> GetUnassingnedTasks()
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.AssigneeId.Equals(null)).ToList();
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
            this.dataBaseContext.HumanTasks.Remove(humantask);
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
            this.dataBaseContext.Entry(humanTask).State = EntityState.Modified;
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

        //TODO: refactor - why method return the same task, witch is in parameters??

        /// <summary>
        /// The add new human task.
        /// </summary>
        /// <param name="humanTask">
        /// The human task.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.HumanTask.
        /// </returns>
        public HumanTask Add(HumanTask humanTask)
        {
            this.dataBaseContext.Entry(humanTask).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
            return humanTask;
        }
    }
}