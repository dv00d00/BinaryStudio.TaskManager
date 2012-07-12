using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using ThirdPartySignup.Models;

namespace ThirdPartySignup.Controllers
{
    public class LinkedInController : Controller
    {

        private readonly UserProcessor userProcessor;

        public LinkedInController(UserProcessor userProcessor)
        {
            this.userProcessor = userProcessor;
        }
        
        private ServiceProviderDescription GetServiceDescription()
        {
            return new ServiceProviderDescription
            {
                AccessTokenEndpoint = new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/accessToken", HttpDeliveryMethods.PostRequest),
                RequestTokenEndpoint = new MessageReceivingEndpoint("https://api.linkedin.com/uas/oauth/requestToken", HttpDeliveryMethods.PostRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://www.linkedin.com/uas/oauth/authorize", HttpDeliveryMethods.PostRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                ProtocolVersion = ProtocolVersion.V10a
            };
        }

        public ActionResult LogonStart()
        {
            var serviceProvider = GetServiceDescription();
            var consumer = new WebConsumer(serviceProvider, TokenHelper.TokenManager);

            // Url to redirect to
            var authUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + "/LinkedIn/LogonEnd");

            // request access
            consumer.Channel.Send(consumer.PrepareRequestUserAuthorization(authUrl, null, null));
            return View();
        }

        public ActionResult LogonEnd(string oath_token)
        {
            var serviceProvider = GetServiceDescription();
            var consumer = new WebConsumer(serviceProvider, TokenHelper.TokenManager);
            var accessTokenResponse = consumer.ProcessUserAuthorization();

            User user = userProcessor.GetUserByLinkedInToken(new Guid(accessTokenResponse.AccessToken));

            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return RedirectToAction("AllManagersWithTasks", "HumanTasks");
            }
            else
            {
                Session["Token"] = accessTokenResponse.AccessToken;
                return RedirectToAction("RegisterLinkedIn");
            }
        }

        public ActionResult RegisterLinkedIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterLinkedIn(RegisterLinkedInModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                bool createStatus = userProcessor.CreateUser(model.UserName, "", model.Email,
                                                             new Guid(Session["Token"].ToString()));
                if (createStatus)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);

                    Session["Token"] = null;
                    return RedirectToAction("AllManagersWithTasks", "HumanTasks");
                }
                else
                {
                    ModelState.AddModelError("", createStatus.ToString());
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
