using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public IEnumerable<Project> GetAll()
        {
            return this.dataBaseContext.Projects.ToList();
        }

        public Project GetById(int projectId)
        {
            return this.dataBaseContext.Projects.Single(it => it.Id == projectId);
        }

        public IEnumerable<User> GetAllUsersInProject(int projectId)
        {
            return dataBaseContext.Projects.First(x => x.Id == projectId).ProjectUsers;
        }

        public IEnumerable<Project> GetAllProjectsForUser(int userId)
        {
            return dataBaseContext.Users.First(x => x.Id == userId).UserProjects;
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
