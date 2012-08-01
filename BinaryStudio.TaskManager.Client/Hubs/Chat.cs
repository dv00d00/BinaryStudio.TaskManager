using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessengR.Models;
using SignalR.Client.Hubs;

namespace MessengR.Models
{
}

namespace MessengR.Client.Hubs
{
    public class TaskMessage
    {
        public string Description;
    }

    public class TaskHub
    {
        private readonly HubConnection connection;

        private readonly IHubProxy hubProxy;

        public event Action<TaskMessage> Message;

        public TaskHub(HubConnection connection)
        {
            this.connection = connection;
            hubProxy = connection.CreateProxy("taskHub");

            hubProxy.On<string>("reciveMessageOnClient", (description) =>
            {
                if (Message != null)
                {
                    Message( new TaskMessage{Description = description});
                }
            });
        }

        public void Join(int projectId, string userNamer)
        {
            hubProxy.Invoke("join", connection.ConnectionId, 0 , userNamer, true);
        }
    }
}
