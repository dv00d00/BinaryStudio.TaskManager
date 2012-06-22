namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Storage;

    public class HumanTaskRepository : IHumanTaskRepository
    {
        private readonly TaskContext taskContext;

        public HumanTaskRepository(TaskContext taskContext)
        {
            this.taskContext = taskContext;
        }

        public IEnumerable<HumanTask> GetForCreator(int creatorId)
        {
            return this.taskContext.HumanTasks.Where(it => it.CreatorId == creatorId).ToList();
        }

        public IEnumerable<HumanTask> GetAllForEmployee(int employeeId)
        {
            return this.taskContext.HumanTasks.Where(it => it.AssigneeId == employeeId).ToList();
        }

        public IEnumerable<HumanTask> GetAll()
        {
            return this.taskContext.HumanTasks.ToList();
        }

        public HumanTask GetById(int humanTaskId)
        {
            return this.taskContext.HumanTasks.Single(it => it.Id == humanTaskId);
        }

        public void Delete(int id)
        {
            HumanTask humantask = this.taskContext.HumanTasks.Single(x => x.Id == id);
            this.taskContext.HumanTasks.Remove(humantask);
            this.taskContext.SaveChanges();
        }

        public void Update(HumanTask humanTask)
        {
            this.taskContext.Entry(humanTask).State = EntityState.Modified;
            this.taskContext.SaveChanges();
        }

        public HumanTask Add(HumanTask humanTask)
        {
            this.taskContext.Entry(humanTask).State = EntityState.Added;
            this.taskContext.SaveChanges();
            return humanTask;
        }
    }
}