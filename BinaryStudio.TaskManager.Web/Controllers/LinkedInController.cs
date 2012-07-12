using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using ThirdPartySignup.Models;

namespace ThirdPartySignup.Controllers
{
    public class LinkedInController : Controller
    {
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

            var doc = XDocument.Load(Server.MapPath("~/App_Data/Users.xml"));
            var query = doc.Descendants("Token").Where(t => t.Value == accessTokenResponse.AccessToken);
            if(query.Any())
            {
                var userId = query.First().Parent.Element("UserID").Value;
                var user = Membership.GetUser(new Guid(userId));
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return RedirectToAction("Index", "Home");
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
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, Guid.NewGuid().ToString(), model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    var doc = XDocument.Load(Server.MapPath("~/App_Data/Users.xml"));
                    var node = new XElement("User",
                                            new XElement("UserID", Membership.GetUser(model.UserName).ProviderUserKey),
                                            new XElement("Token", Session["Token"]));
                    doc.Root.Add(node);
                    doc.Save(Server.MapPath("~/App_Data/Users.xml"));
                    Session["Token"] = null;
                    return RedirectToAction("Index", "Home");
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
