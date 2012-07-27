// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfileController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProfileController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Controllers
{
    using System.Web;
    using System.Web.Mvc;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Models;

    using System.Linq;

    /// <summary>
    /// The profile controller.
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        /// <summary>
        /// The user processor.
        /// </summary>
        private readonly IUserProcessor userProcessor;

        private readonly IProjectProcessor projectProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="userProcessor">
        /// The user processor.
        /// </param>
        public ProfileController(IUserProcessor userProcessor, IProjectProcessor projectProcessor)
        {
            this.userProcessor = userProcessor;
            this.projectProcessor = projectProcessor;
        }

        /// <summary>
        /// The get image in profile.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult GetImageInProfile(string userName)
        {
            User user = this.userProcessor.GetUserByName(userName);
            if (user != null)
            {
                return this.File(user.ImageData, user.ImageMimeType);
            }

            return null;
        }

        /// <summary>
        /// The profile changes.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="image">
        /// The image.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        public ActionResult ProfileChanges(ProfileModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    model.ImageMimeType = image.ContentType;
                    model.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(model.ImageData, 0, image.ContentLength);
                }
            }
            return this.RedirectToAction("Profile");
        }

        /// <summary>
        /// The profile.
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult Profile()
        {
            var userId = this.userProcessor.GetUserByName(User.Identity.Name).Id;
            var model = new ProfileModel
                { InvitationsCount = this.projectProcessor.GetAllInvitationsToUser(userId).Count(),UserName = this.userProcessor.GetUserByName(User.Identity.Name).UserName};
            return this.View(model);
        }
    }
}
