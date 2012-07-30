namespace BinaryStudio.TaskManager.Logic.Core
{
    using global::SignalR.Hubs;

    /// <summary>
    /// Interface to be implemented by global host components
    /// </summary>
    public interface IGlobalHost
    {
        dynamic GetContext<TContext>() where TContext : IHub;
    }
}