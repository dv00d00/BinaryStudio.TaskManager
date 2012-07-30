namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to be implemented by connection provider components
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// Gets the active connections.
        /// </summary>
        IEnumerable<ClientConnection> ActiveConnections { get; }

        /// <summary>
        /// Gets the project connections.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        IEnumerable<ClientConnection> GetProjectConnections(int projectId);
    }
}