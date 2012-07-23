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

        //public IQueryable<Project> All
        //{
        //    get { return projectsAndUsers.Project; }
        //}

        public IEnumerable<Project> GetAll()
        {
            return this.dataBaseContext.Projects.ToList();
        }

        public Project GetById(int projectId)
        {
            return this.dataBaseContext.Projects.Single(it => it.Id == projectId);
        }

        public ProjectsAndUsers AddNewUserToProject(ProjectsAndUsers projectsAndUsers)
        {
            this.dataBaseContext.Entry(projectsAndUsers).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
            return projectsAndUsers;
        }

        public IEnumerable<ProjectsAndUsers> GetAllUsersInProject(int projectId)
        {
            return this.dataBaseContext.ProjectsAndUserses.Where(it => it.Project.Id == projectId).ToList();
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
