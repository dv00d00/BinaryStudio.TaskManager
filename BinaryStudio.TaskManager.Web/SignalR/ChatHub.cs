using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Web.SignalR
{
    public class Chat : Hub
    {
        public void Distribute(dynamic message)
        {
            Clients.Receive(message);
        }
    }
}