<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProjectRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Core
=======
﻿namespace BinaryStudio.TaskManager.Logic.Core
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The project repository.
    /// </summary>
    public class ProjectRepository : IProjectRepository
    {
        /// <summary>
        /// The data base context.
        /// </summary>
        private readonly DataBaseContext dataBaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
        /// </summary>
        /// <param name="dataBaseContext">
        /// The data base context.
        /// </param>
        public ProjectRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.Project].
        /// </returns>
        public IEnumerable<Project> GetAll()
        {
            return this.dataBaseContext.Projects.ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.Project.
        /// </returns>
        public Project GetById(int projectId)
        {
            return this.dataBaseContext.Projects.Single(it => it.Id == projectId);
        }

        /// <summary>
        /// The get all users in project.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.User].
        /// </returns>
        public IEnumerable<User> GetAllUsersInProject(int projectId)
        {
            return this.dataBaseContext.Projects.First(x => x.Id == projectId).ProjectUsers;
        }

        /// <summary>
        /// The get all projects for user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; BinaryStudio.TaskManager.Logic.Domain.Project].
        /// </returns>
        public IEnumerable<Project> GetAllProjectsForUser(int userId)
        {
            return this.dataBaseContext.Users.First(x => x.Id == userId).UserProjects;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="project">
        /// The project.
        /// </param>
        public void Add(Project project)
        {
            this.dataBaseContext.Entry(project).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        public void Delete(int projectId)
        {
            var project = this.dataBaseContext.Projects.Single(x => x.Id == projectId);
            this.dataBaseContext.Projects.Remove(project);
            this.dataBaseContext.SaveChanges();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="project">
        /// The project.
        /// </param>
        public void Update(Project project)
        {
            this.dataBaseContext.Entry(project).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        public void CreateInvitationUserInProject(Invitation invitation)
        {
            this.dataBaseContext.Entry(invitation).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public void UpdateInvitation(Invitation invitation)
        {
            this.dataBaseContext.Entry(invitation).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        public IEnumerable<Invitation> GetAllInvitationsForUser(int userId)
        {
            var invitations =
                this.dataBaseContext.Invitations.Where(x => x.UserId == userId).Where(x => x.IsInvitationSended).Select(
                    x => x.IsInvitationConfirmed == false);
            return (IEnumerable<Invitation>)invitations;
        }
    }
}
