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
    /// <summary>
    /// Strongly typed client side proxy for the server side Hub (Similar to add service reference).
    /// </summary>
    public class Chat
    {
        private readonly IHubProxy _chat;

        public event Action<User> UserOnline;

        public event Action<User> UserOffline;

        public event Action<Message> Message;

        public Chat(HubConnection connection)
        {
            _chat = connection.CreateProxy("Chat");

            _chat.On<User>("markOnline", user =>
            {
                if (UserOnline != null)
                {
                    UserOnline(user);
                }
            });

            _chat.On<User>("markOffline", user =>
            {
                if (UserOffline != null)
                {
                    UserOffline(user);
                }
            });

            _chat.On<Message>("addMessage", message =>
            {
                if (Message != null)
                {
                    Message(message);
                }
            });
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return _chat.Invoke<IEnumerable<User>>("GetUsers");
        }

        public Task<User> GetUser(string userName)
        {
            return _chat.Invoke<User>("GetUser", userName);
        }

        public Task<Message> SendMessage(string userName, string message)
        {
            return _chat.Invoke<Message>("Send", userName, message);
        }
    }

    public class TaskMessage
    {
        public int TaskId { get; set; }

        public int MoveToId { get; set; }
    }

    public class TaskHub
    {
        private readonly HubConnection connection;

        private readonly IHubProxy _chat;

        public event Action<TaskMessage> Message;

        public TaskHub(HubConnection connection)
        {
            this.connection = connection;
            _chat = connection.CreateProxy("taskHub");

            _chat.On<int, int>("taskMoved", (taskId, destinationId) =>
            {
                if (Message != null)
                {
                    Message( new TaskMessage{MoveToId = destinationId, TaskId = taskId});
                }
            });
        }

        public void Join(int projectId, string userNamer)
        {
            _chat.Invoke("join", connection.ConnectionId, projectId, userNamer, false);
        }
    }
}
