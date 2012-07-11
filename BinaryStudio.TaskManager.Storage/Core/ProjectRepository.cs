using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public ProjectRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        public IEnumerable<Project> GetCreatorId(int creatorId)
        {
            return this.dataBaseContext.Projects.Where(it => it.CreatorId == creatorId).ToList();
        }

        public IList<Project> GetAllProjectsForUser(int userId)
        {
            return this.dataBaseContext.Projects.Where(it => it.ProjectsAndUsersId == userId).ToList();
        }

        public IEnumerable<Project> GetAll()
        {
            return this.dataBaseContext.Projects.ToList();
        }

        public Project GetById(int projectId)
        {
            return this.dataBaseContext.Projects.Single(it => it.Id == projectId);
        }

        public void Add(Project project)
        {
            this.dataBaseContext.Entry(project).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public void Delete(int projectId)
        {
            Project project = this.dataBaseContext.Projects.Single(x => x.Id == projectId);
            this.dataBaseContext.Projects.Remove(project);
            this.dataBaseContext.SaveChanges();
        }

        public void Update(Project project)
        {
            this.dataBaseContext.Entry(project).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }
    }
}
