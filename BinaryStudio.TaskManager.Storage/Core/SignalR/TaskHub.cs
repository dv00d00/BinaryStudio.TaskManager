using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Core.SignalR;
using SignalR.Hubs;

namespace BinaryStudio.TaskManager.Web.SignalR
{
    [HubName("taskHub")]
    public class TaskHub : Hub
    {
        private readonly IUserProcessor userProcessor;

        public TaskHub(IUserProcessor userProcessor)
        {
            this.userProcessor = userProcessor;
        }

        public void Distribute(dynamic message)
        {
            Clients.Receive(message);
        }

        public void Join(string id, int projectId, string userName, bool isWPFClientConnection)
        {
            var user = userProcessor.GetUserByName(userName);
            int? userId = user == null ? null : (int?)user.Id;
            SignalRClients.AddConnection(id, projectId, userName, userId, isWPFClientConnection);
        }

        public void ChangeProject(string connectionId, int projectId)
        {
            SignalRClients.ChangeConnectionProject(connectionId, projectId);
        }

        public void LoginWithClient(string id, string userName, string password)
        {
            bool status = userProcessor.LogOnUser(userName, password);
            Clients[id].ReciveClientLogonStatus(status);
        }

    }
}

