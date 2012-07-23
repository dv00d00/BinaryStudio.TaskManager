using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Web.SignalR
{
    public class ChatHub : Hub
    {
        public void Distribute(string message)
        {
            Clients.Receive(message);
        }
    }
}