namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ProjectProcessor : IProjectProcessor
    {
        /// <summary>
        /// The project repository.
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// The user repository.
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectProcessor"/> class.
        /// </summary>
        /// <param name="projectRepository">
        /// The project repository.
        /// </param>
        public ProjectProcessor(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// The create default project.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void CreateDefaultProject(User user)
        {
            this.CreateProject(user, "Home Project", string.Empty);
        }

        /// <summary>
        /// Send invitation to user in project.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        public void InviteUserInProject(int userId, int projectId)
        {
            var invitation = new Invitation
                {
                    UserId = userId,
                    ProjectId = projectId,
                    IsInvitationSended = true,
                    IsInvitationConfirmed = false
                };
            this.projectRepository.CreateInvitationUserInProject(invitation);
        }

        /// <summary>
        /// The remove user from project.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        public void RemoveUserFromProject(int userId, int projectId)
        {
            var user = this.userRepository.GetById(userId);
            user.UserProjects.Remove(this.projectRepository.GetById(projectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(projectId);
            project.ProjectUsers.Remove(this.userRepository.GetById(userId));
            this.projectRepository.Update(project);
        }

        /// <summary>
        /// The create custom project.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="projectName">
        /// The project name.
        /// </param>
        /// <param name="projectDescription">
        /// The project description.
        /// </param>
        public void CreateProject(User user, string projectName, string projectDescription)
        {
            var project = new Project
                {
                    CreatorId = user.Id,
                    Creator = user,
                    Created = DateTime.Now,
                    Name = projectName,
                    Description = projectDescription
                };
            this.projectRepository.Add(project);
        }
    }
}