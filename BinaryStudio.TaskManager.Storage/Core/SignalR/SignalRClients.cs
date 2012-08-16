using System.Collections.Generic;

namespace BinaryStudio.TaskManager.Logic.Core.SignalR
{
    using System.Linq;

    public static class SignalRClients
    {
        public static IList<ClientConnection> Connections = new List<ClientConnection>();

        public static void AddConnection (string connectionId,int projectId, string userName, int? userId, bool isWPFClient)
        {
            Connections.Add(new ClientConnection
                                {
                                    ConnectionId = connectionId,
                                    ProjectId = projectId,
                                    UserName = userName,
                                    IsWPFClient = isWPFClient,
                                    UserId = userId
                                });
        }

        public static void ChangeConnectionProject(string connectionId, int projectId)
        {
            var connection = Connections.Where(con => con.ConnectionId == connectionId).SingleOrDefault();
            Connections.Remove(connection);
            connection.ProjectId = projectId;
            Connections.Add(connection);
        }
    }
}
