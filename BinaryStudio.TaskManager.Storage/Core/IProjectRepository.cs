using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll();

        Project GetById(int projectId);

        ProjectsAndUsers AddNewUserToProject(ProjectsAndUsers projectsAndUsers);

        IEnumerable<ProjectsAndUsers> GetAllUsersInProject(int projectId);

        void Add(Project project);

        void Delete(int projectId);

        void Update(Project project);
    }
}