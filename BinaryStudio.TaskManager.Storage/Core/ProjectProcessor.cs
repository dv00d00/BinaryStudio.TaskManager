namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The project processor.
    /// </summary>
    public class ProjectProcessor : IProjectProcessor
    {
        /// <summary>
        /// The project repository.
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectProcessor"/> class.
        /// </summary>
        /// <param name="projectRepository">
        /// The project repository.
        /// </param>
        /// <param name="userRepository">
        /// The user Repository.
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
        /// The current user.
        /// </param>
        public void CreateDefaultProject(User user)
        {
            this.CreateProject(user, "Home Project", string.Empty);
        }

        /// <summary>
        /// The invite user in project.
        /// </summary>
        /// <param name="senderId">
        /// The sender id.
        /// </param>
        /// <param name="projectId">
        /// The project id.
        /// </param>
        /// <param name="receiverId">
        /// The receiver id.
        /// </param>
        public void InviteUserInProject(int senderId, int projectId, int receiverId)
        {            
            var invitations = this.GetAllInvitationsToProject(projectId);
            if (invitations.Any(oneInvitation => oneInvitation.ReceiverId == receiverId && oneInvitation.ProjectId == projectId))
            {
                //return;
            }

            var invitation = new Invitation
                {
                    ReceiverId = receiverId,
                    SenderId = senderId,
                    ProjectId = projectId,
                    IsInvitationConfirmed = false
                };
            this.projectRepository.AddInvitation(invitation);
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
            return this.projectRepository.GetAllUsersInProject(projectId);
        }

        /// <summary>
        /// The confirm invitation in project.
        /// </summary>
        /// <param name="invitation">
        ///   The invitation.
        /// </param>
        public void ConfirmInvitationInProject(int invitationId)
        {
            var invitation = this.projectRepository.GetInvitationById(invitationId);
            var projectId = invitation.ProjectId;
            var receiverId = invitation.ReceiverId;

            var user = this.userRepository.GetById(receiverId);
            user.UserProjects.Add(this.projectRepository.GetById(projectId));
            this.userRepository.UpdateUser(user);

            var project = this.projectRepository.GetById(projectId);
            project.ProjectUsers.Add(this.userRepository.GetById(receiverId));
            this.projectRepository.Update(project);

            invitation.IsInvitationConfirmed = true;
            this.projectRepository.UpdateInvitation(invitation);
        }

        public IEnumerable<Invitation> GetAllInvitationsToUser(int userId)
        {
            return this.projectRepository.GetAllInvitationsForUser(userId);
        }

        public Project GetProjectById(int projectId)
        {
            return this.projectRepository.GetById(projectId);
        }

        public void RefuseFromParticipateProject(int invitationId)
        {
            var invitation = this.projectRepository.GetInvitationById(invitationId);
            this.projectRepository.DeleteInvitation(invitation);
        }

        public IEnumerable<Invitation> GetAllInvitationsToProject(int projectId)
        {
            return this.projectRepository.GetAllInvitationsToProject(projectId);
        }

        public IEnumerable<User> GetUsersAndCreatorInProject(int projectId)
        {
            List<User>  users = new List<User>(this.GetAllUsersInProject(projectId));
            users.Add(this.GetCreator(projectId));
            return users;
        }

        public User GetCreator(int projectId)
        {
            return this.GetProjectById(projectId).Creator;
        }


        /// <summary>
        /// The create custom project with project name and description.
        /// </summary>
        /// <param name="user">
        /// The user tied to project.
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

        public IEnumerable<Project> GetAllProjectsForTheirCreator(int userId)
        {
           return projectRepository.GetAllProjectsForTheirCreator(userId);
        }

        public IEnumerable<Project> GetAllProjectsForUser(int userId)
        {
            return projectRepository.GetAllProjectsForUser(userId);
        }
    }
}