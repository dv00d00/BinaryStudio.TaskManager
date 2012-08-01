namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Core.SignalR;

    public class ConnectionProvider : IConnectionProvider
    {
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

        public IEnumerable<ClientConnection> GetNewsConnectionsForUser(int userId)
        {
            return SignalRClients.Connections.Where(it => it.UserId == userId && it.IsNewsConnection);
        }

        public IEnumerable<ClientConnection> GetConnetionsForUser(string userName)
        {
            return SignalRClients.Connections.Where(it => it.UserName == userName);
        }
    }
}