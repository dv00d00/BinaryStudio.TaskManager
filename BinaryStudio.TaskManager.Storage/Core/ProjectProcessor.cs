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
        /// Initializes a new instance of the <see cref="ProjectProcessor"/> class.
        /// </summary>
        /// <param name="projectRepository">
        /// The project repository.
        /// </param>
        public ProjectProcessor(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
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