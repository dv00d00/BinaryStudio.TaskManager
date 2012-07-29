
using BinaryStudio.TaskManager.Logic.Core.SignalR;
using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Web.SignalR
{
    [HubName("taskHub")]
    public class TaskHub : Hub
    {
        public void Distribute(dynamic message)
        {
            Clients.Receive(message);
        }

        public void Join(string id)
        {
            SignalRCliets.AddConnection(id);
        }
    }
}