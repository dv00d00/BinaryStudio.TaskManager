using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.TaskManager.Logic.Core.SignalR
{
    public static class SignalRClients
    {
        public static IList<ClientConnection> Connections = new List<ClientConnection>();

        public static void AddConnection (string connectionId,int projectId)
        {
            Connections.Add(new ClientConnection
                                {
                                    ConnectionId = connectionId,
                                    ProjectId = projectId
                                });
        }
    }
}
