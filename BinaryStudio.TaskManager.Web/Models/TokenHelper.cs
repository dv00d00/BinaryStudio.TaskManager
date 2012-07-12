using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ThirdPartySignup.Models
{
    public class TokenHelper
    {
        public static InMemoryTokenManager TokenManager = new InMemoryTokenManager(ConfigurationManager.AppSettings["consumerKey"], ConfigurationManager.AppSettings["consumerSecret"]);
    }
}