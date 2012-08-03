using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Input;
using MessengR.Client.Common;
using MessengR.Client.Hubs;
using MessengR.Models;
using Microsoft.Practices.Prism.Events;
using SignalR.Client.Hubs;
using WPFTaskbarNotifierExample;

namespace MessengR.Client.ViewModel
{
    using System.Windows.Threading;

    public class MainViewModel : ViewModelBase
    {
        #region SignalR Objects
        private HubConnection _connection;
        private readonly SynchronizationContext _syncContext;
        private TaskHub taskHub;
        private ExampleTaskbarNotifier taskbarNotifier = new ExampleTaskbarNotifier();
        #endregion

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private ContactViewModel _me;
        public ContactViewModel Me
        {
            get { return _me; }
            set
            {
                _me = value;
                OnPropertyChanged("Me");
            }
        }
        private ObservableCollection<ContactViewModel> _contacts;
        public ObservableCollection<ContactViewModel> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                OnPropertyChanged("Contacts");
            }
        }

        private ObservableCollection<Message> _conversations;
        public ObservableCollection<Message> Conversations
        {
            get { return _conversations; }
            set
            {
                _conversations = value;
                OnPropertyChanged("Conversations");
            }
        }
        private readonly ChatSessionsViewModel _chatSessions = new ChatSessionsViewModel();

        public MainViewModel() { }

        public MainViewModel(string name)
        {
            Name = name;
            _chatSessions.SendMessage += OnSendMessage;
            
            // Store the sync context from the ui thread so we can post to it
            _syncContext = SynchronizationContext.Current;
            InitializeConnection(ConfigurationManager.AppSettings["HostUrl"]);

        }

        private void InitializeConnection(string url)
        {
            _connection = new HubConnection(url) { CookieContainer = new CookieContainer() };

            // Get a reference to the chat proxy
            taskHub = new TaskHub(_connection);

            this.Conversations = new ObservableCollection<Message>();
            
            

            taskHub.Message += message =>
                {
                    this._syncContext.Post(
                        state =>
                            {
                                this.Conversations.Add(new Message { Value = message.Description});
                            },
                        null);
                    
                };

            // Start the connection
            _connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Show a connection error here
                }
                else
                {
                    taskHub.Join(2, Name);
                }

            });

            // Fire events on the ui thread
            taskHub.Message += message => _syncContext.Post(x => OnMessage(message), null);
        }

        private void OnMessage(TaskMessage message)
        {
            taskbarNotifier = new ExampleTaskbarNotifier();
            taskbarNotifier.Show();
            taskbarNotifier.ShowInTaskbar = true;
            taskbarNotifier.OpeningMilliseconds = 1000;
            taskbarNotifier.StayOpenMilliseconds = 3000;
            taskbarNotifier.HidingMilliseconds = 1000;
            // Starts a new conversation with message.From if not started,
            // otherwise, it will add a message to the conversation window with message.From.
            this.taskbarNotifier.NotifyContent.Add(new NotifyObject(message.Description, ""));

            // Tell the TaskbarNotifier to open.
            this.taskbarNotifier.Notify();

            taskbarNotifier.ShowInTaskbar = true;
        }

        private void OnUserStatusChange(User user)
        {
            //Mark user as online/offline
            if (Contacts != null)
            {
                var contact = Contacts.SingleOrDefault(u => u.User.Name == user.Name);
                if (contact != null)
                {
                    Contacts.Remove(contact);
                    Contacts.Add(new ContactViewModel(user, Me));
                }
            }
        }

        public void OnOpenChat(object sender, EventArgs e)
        {
            if (sender is ContactViewModel)
            {
                _chatSessions.StartNewSession((sender as ContactViewModel).User, Me.User);
            }
        }

        void OnSendMessage(object sender, EventArgs e)
        {
            if (sender is ChatSessionViewModel)
            {
                var chat = sender as ChatSessionViewModel;
            }
        }
    }
}
