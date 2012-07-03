using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly Logic.Core.IUserRepository userRepository;

           public AccountController(IUserRepository userRepository)
           {
               this.userRepository = userRepository;
           }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
               if (userRepository.LogOn(model.UserName,model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Введенное имя пользователя или пароль неверны.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
