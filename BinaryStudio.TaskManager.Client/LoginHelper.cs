using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MessengR.Client.Hubs;
using SignalR.Client.Hubs;

namespace MessengR.Client
{
    public static class LoginHelper
    {
        /// <summary>
        /// Logs into the MessengR service using forms auth and retrives an auth cookie.
        /// </summary>
        public static Task<AuthenticationResult> LoginAsync(string url, string userName, string password)
        {
            HubConnection connection = new HubConnection(url) { CookieContainer = new CookieContainer() }; ;
            TaskHub taskHub = new TaskHub(connection);
            var tcs = new TaskCompletionSource<AuthenticationResult>();
            connection.Start().ContinueWith(task =>
             {
              taskHub.Logon(userName, password).ContinueWith(logon => 
                  taskHub.LogonStatus += logonStatus => tcs.SetResult(new AuthenticationResult
                                                                          {
                                                                              Status = logonStatus.Status
                                                                          }));
             });
            
            return tcs.Task;
        }
    }
}
