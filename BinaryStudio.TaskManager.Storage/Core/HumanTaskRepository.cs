using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class HumanTaskRepository : IHumanTaskRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public HumanTaskRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        public IEnumerable<HumanTask> GetForCreator(int creatorId)
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.CreatorId == creatorId).ToList();
        }

        public IList<HumanTask> GetAllTasksForEmployee(int employeeId)
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.AssigneeId == employeeId).ToList();
        }

        public IList<HumanTask> GetUnassingnedTasks()
        {
            return this.dataBaseContext.HumanTasks.Where(it => it.AssigneeId.Equals(null)).ToList();
        }

        public IEnumerable<HumanTask> GetAll()
        {
            return this.dataBaseContext.HumanTasks.ToList();
        }

        public HumanTask GetById(int humanTaskId)
        {
            return this.dataBaseContext.HumanTasks.Single(it => it.Id == humanTaskId);
        }

        public void Delete(int humanTaskId)
        {
            HumanTask humantask = this.dataBaseContext.HumanTasks.Single(x => x.Id == humanTaskId);
            this.dataBaseContext.HumanTasks.Remove(humantask);
            this.dataBaseContext.SaveChanges();
        }

        public void Update(HumanTask humanTask)
        {
            this.dataBaseContext.Entry(humanTask).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        //TODO: refactor - why method return the same task, witch is in parameters??
        public HumanTask Add(HumanTask humanTask)
        {
            this.dataBaseContext.Entry(humanTask).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
            return humanTask;
        }
    }
}