<<<<<<< HEAD:BinaryStudio.TaskManager.Storage/SignalR/TaskHub.cs
using BinaryStudio.TaskManager.Logic.Core;
using SignalR.Hubs;
namespace BinaryStudio.TaskManager.Logic.SignalR
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

        public void MoveTask(int taskId, int moveToId, ClientConnection clientConnection)
        {
            moveToId = moveToId == -1 ? 0 : moveToId;
            string senderConnectionId = clientConnection.ClientId;
            Clients.TaskMoved(taskId, moveToId,senderConnectionId);
        }
    }
=======
using BinaryStudio.TaskManager.Logic.Core;
using SignalR.Hubs;
namespace BinaryStudio.TaskManager.Logic.SignalR
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

        public void MoveTask(int taskId, int moveToId, ClientConnection clientConnection)
        {
            moveToId = moveToId == -1 ? 0 : moveToId;
            string senderConnectionId = clientConnection.ClientId;
            Clients.TaskMoved(taskId, moveToId,senderConnectionId);
        }
    }
>>>>>>> 3be595d6356143f594f5df5382171d010d012ba6:BinaryStudio.TaskManager.Storage/SignalR/TaskHub.cs
}