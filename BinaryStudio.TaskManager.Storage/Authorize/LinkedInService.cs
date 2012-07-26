using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinkedIn.OAuth;
using LinkedIn.OAuth.Model;

namespace BinaryStudio.TaskManager.Logic.Authorize
{
    public class LinkedInService : ILinkedInService
    {
        public void BeginAuthorization(string callback)
        {
            LinkedInSession session = LinkedInSession.RetrieveFromUserSession();
            if (session == null)
            {
                OAuthToken token = OAuthManager.Current.CreateToken(callback: callback);
                OAuthManager.Current.BeginAuth(token, endResponse: true, displayAllowDenyScreen: true);
            }
        }

        public LinkedInProfile EndAuthorization()
        {
            OAuthToken token = OAuthManager.Current.GetTokenInCallback();
            LinkedInSession session = OAuthManager.Current.CompleteAuth(token);
            if (session.IsAuthorized())
            {
                session.StoreInUserSession();
                return session.GetProfile();
            }
            return null;
        }


        public LinkedInProfile GetUserProfile()
        {
            LinkedInSession session = LinkedInSession.RetrieveFromUserSession();
            if (session != null)
            {
                if (session.IsAuthorized())
                {
                    session.StoreInUserSession();
                    return session.GetProfile();
                }
                return null;
            }
            return null;
        }

        public bool IsAutorized()
        {
            LinkedInSession session = LinkedInSession.RetrieveFromUserSession();
            if (session != null)
            {
                return session.IsAuthorized();
            }
            return false;
        }

    }
}
