namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public interface IConnectionProvider
    {
        IEnumerable<ClientConnection> ActiveConnections { get; }

        IEnumerable<ClientConnection> GetProjectConnections(int projectId);

        IEnumerable<ClientConnection> GetNewsConnectionsForUser(int userId);

        IEnumerable<ClientConnection> GetConnetionsForUser(string userName);
    }
}