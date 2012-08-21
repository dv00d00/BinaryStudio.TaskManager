using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessengR.Models;
using SignalR.Client.Hubs;
using WPFTaskbarNotifierExample;

namespace MessengR.Models
{
}

namespace MessengR.Client.Hubs
{
    public class TaskMessage
    {
        public string Description;
    }

    public class LogonStatusMessage
    {
        public bool Status;
    }

    public class TaskHub
    {
        private readonly HubConnection connection;

        private readonly IHubProxy hubProxy;

        public event Action<TaskMessage,int> Message;

        public event Action<LogonStatusMessage> LogonStatus;

        public TaskHub(HubConnection connection)
        {
            this.connection = connection;
            hubProxy = connection.CreateProxy("taskHub");

            hubProxy.On<string,int>("reciveMessageOnClient", (description,projectId) =>
            {
                if (Message != null)
                {
                    Message( new TaskMessage{Description = description},projectId);
                }
            });

            hubProxy.On<bool>("reciveClientLogonStatus", (status) =>
            {
                if (LogonStatus != null)
                {
                    LogonStatus(new LogonStatusMessage { Status = status });
                }
            });
        }

        public void Join(int projectId, string userNamer)
        {
            hubProxy.Invoke("join", connection.ConnectionId, 0 , userNamer, true);
        }

        public Task Logon(string userName, string password)
        {
           return hubProxy.Invoke("loginWithClient", connection.ConnectionId, userName, password);
        }
    }
}
