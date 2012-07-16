using System;
using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Authorize;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using DotNetOpenAuth.OAuth;
using LinkedIn.OAuth;
using LinkedIn.OAuth.Model;
using ThirdPartySignup.Models;

namespace ThirdPartySignup.Controllers
{
    public class LinkedInController : Controller
    {
        private readonly LinkedInService _linkedInService;
        private readonly UserProcessor _userProcessor;

        public LinkedInController(UserProcessor userProcessor, LinkedInService linkedInService)
        {
            this._userProcessor = userProcessor;
            this._linkedInService = linkedInService;
        }


        public ActionResult LogonStart()
        {
            string authUrl =Request.Url.Scheme + "://" + Request.Url.Authority + "/LinkedIn/LogonEnd";
            if(!_linkedInService.IsAutorized())
            {
                _linkedInService.BeginAuthorization(authUrl);
            }
            else
            {
                LinkedInProfile profile = _linkedInService.GetUserProfile();
                User user = _userProcessor.GetUserByLinkedInId(profile.UserId);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("AllManagersWithTasks", "HumanTasks");
                }
                else
                {
                    return RedirectToAction("RegisterLinkedIn");
                }
            }
            return null;
        }

        public ActionResult LogonEnd()
        {
            LinkedInProfile profile = _linkedInService.EndAuthorization();
            User user = _userProcessor.GetUserByLinkedInId(profile.UserId);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return RedirectToAction("AllManagersWithTasks", "HumanTasks");
            }
            else
            {
                return RedirectToAction("RegisterLinkedIn");
            }
        }



        public ActionResult RegisterLinkedIn()
        {
            LinkedInProfile profile = _linkedInService.GetUserProfile();
            RegisterLinkedInModel model = new RegisterLinkedInModel()
                                              {
                                                  FirstName = profile.Firstname,
                                                  LastName = profile.Lastname,
                                                  UserName = profile.Firstname+ " " + profile.Lastname,
                                                  LinkedInId = profile.UserId
                                              };
            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterLinkedIn(RegisterLinkedInModel model)
        {
            

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                bool createStatus = _userProcessor.CreateUser(model.UserName, "", model.Email,model.LinkedInId);
                if (createStatus)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,true);
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
