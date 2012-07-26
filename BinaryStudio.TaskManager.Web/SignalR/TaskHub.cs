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
            Groups.Add(id, "simpleGroup");
            Distribute(id);
        }

        public void MoveTask(int taskId, int moveToId, string senderConnectionId)
        {
            Clients.TaskMoved(taskId, moveToId,senderConnectionId);
        }
    }
}