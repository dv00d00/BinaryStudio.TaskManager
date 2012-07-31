namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Core.SignalR;

    public class ConnectionProvider : IConnectionProvider
    {
        /// <summary>
        /// Gets the active connections.
        /// </summary>
        public IEnumerable<ClientConnection> ActiveConnections
        {
            get
            {
                return SignalRClients.Connections;
            }
        }

        /// <summary>
        /// Gets the project connections.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public IEnumerable<ClientConnection> GetProjectConnections(int projectId)
        {
            return SignalRClients.Connections.Where(it => it.ProjectId == projectId);
        }
    }
}