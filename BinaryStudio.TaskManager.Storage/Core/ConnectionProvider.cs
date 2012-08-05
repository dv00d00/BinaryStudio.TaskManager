namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Core.SignalR;

    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IProjectRepository projectRepository;

        public ConnectionProvider(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public IEnumerable<ClientConnection> ActiveConnections
        {
            get
            {
                return SignalRClients.Connections;
            }
        }

        public IEnumerable<ClientConnection> GetProjectConnections(int projectId)
        {
            return SignalRClients.Connections.Where(it => it.ProjectId == projectId);
        }

        public IEnumerable<ClientConnection> GetConnetionsForUser(string userName)
        {
            return SignalRClients.Connections.Where(it => it.UserName == userName);
        }
        public IEnumerable<ClientConnection> GetWPFConnectionsForProject(int projectId)
        {
            IList<ClientConnection> connections = new List<ClientConnection>();
            var users = projectRepository.GetAllUsersInProject(projectId);
            foreach (var user in users)
            {
                connections.Concat(SignalRClients.Connections.Where(x => x.UserName == user.UserName).ToList());
            }
            var creator = projectRepository.GetCreatorForProject(projectId);
            List<ClientConnection> creatorConnections =
                SignalRClients.Connections.Where(x => x.UserName == creator.UserName && x.IsWPFClient).ToList();

            return connections.Concat(creatorConnections); 
        }

        public IEnumerable<ClientConnection> GetClientConnectionsForUser(int userId)
        {
            return SignalRClients.Connections.Where(it => it.IsWPFClient && it.UserId == userId);
        }
    }
}