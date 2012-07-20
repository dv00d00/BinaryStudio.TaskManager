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
    using System.Globalization;

    /// <summary>
    /// Controller create feature login with LinkedIn account
    /// </summary>
    public class LinkedInController : Controller
    {
        /// <summary>
        /// The linkedIn service
        /// </summary>
        private readonly LinkedInService linkedInService;

        /// <summary>
        /// The user processor.
        /// </summary>
        private readonly UserProcessor userProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedInController"/> class.
        /// </summary>
        /// <param name="userProcessor">
        /// The user processor.
        /// </param>
        /// <param name="linkedInService">
        /// The linked in service.
        /// </param>
        public LinkedInController(UserProcessor userProcessor, LinkedInService linkedInService)
        {
            this.userProcessor = userProcessor;
            this.linkedInService = linkedInService;
        }

        /// <summary>
        /// Logons the start.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogonStart()
        {
            var authUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/LinkedIn/LogonEnd";
            if (!this.linkedInService.IsAutorized())
            {
                this.linkedInService.BeginAuthorization(authUrl);
            }
            else
            {
                LinkedInProfile profile = this.linkedInService.GetUserProfile();
                User user = this.userProcessor.GetUserByLinkedInId(profile.UserId);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return this.RedirectToAction("AllManagersWithTasks", "HumanTasks");
                }
                else
                {
                    return this.RedirectToAction("RegisterLinkedIn");
                }
            }
            return null;
        }

        /// <summary>
        /// Logons the end.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult LogonEnd()
        {
            LinkedInProfile profile = this.linkedInService.EndAuthorization();
            User user = this.userProcessor.GetUserByLinkedInId(profile.UserId);
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

        /// <summary>
        /// The register linked in.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult RegisterLinkedIn()
        {
            LinkedInProfile profile = this.linkedInService.GetUserProfile();
            RegisterLinkedInModel model = new RegisterLinkedInModel()
                                              {
                                                  FirstName = profile.Firstname,
                                                  LastName = profile.Lastname,
                                                  UserName = profile.Firstname+ " " + profile.Lastname,
                                                  LinkedInId = profile.UserId
                                              };
            return View(model);
        }

        /// <summary>
        /// The register linked in.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult RegisterLinkedIn(RegisterLinkedInModel model)
        {            
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var createStatus = this.userProcessor.CreateUser(model.UserName, string.Empty, model.Email,model.LinkedInId);
                if (createStatus)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,true);
                    return this.RedirectToAction("AllManagersWithTasks", "HumanTasks");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, false.ToString(CultureInfo.InvariantCulture));
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }
    }
}