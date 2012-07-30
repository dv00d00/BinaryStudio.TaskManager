namespace BinaryStudio.TaskManager.Logic.Core
{
    using global::SignalR;
    using global::SignalR.Hubs;

    public class GlobalHostImpl : IGlobalHost
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <returns></returns>
        public dynamic GetContext<TContext>() where TContext : IHub
        {
            return GlobalHost.ConnectionManager.GetHubContext<TContext>();
        }
    }
}