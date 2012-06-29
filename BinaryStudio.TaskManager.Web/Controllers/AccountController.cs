using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {

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
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
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

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Неверен текущий пароль или новый пароль с ошибками.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Пользователь с таким именем уже существует. Пожалуйста, введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Пользователь с таким e-mail уже существует. Пожалуйста, введите другой e-mail.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Неверный пароль. Пожалуйста, введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Введен несуществующий e-mail. Пожалуйста, проверьте введенное значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Контрольный ответ для восстановления пароля неверный. Пожалуйста, проверьте введенное значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Контрольный вопрос для восстановления пароля неверный. Пожалуйста, проверьте введенное значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Неверное имя пользователя. Пожалуйста, проверьте введенное значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Проверки подлинности возвращается сообщение об ошибке. Пожалуйста, проверьте Вашу запись и попробуйте еще раз. Если проблема не устраняется, обратитесь к Вашему системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос о создании пользователя был отменен. Пожалуйста, проверьте Вашу запись и попробуйте еще раз. Если проблема не устраняется, обратитесь к Вашему системному администратору.";

                default:
                    return "Возникла неизвестная ошибка. Пожалуйста, проверьте Вашу запись и попробуйте еще раз. Если проблема не устраняется, обратитесь к Вашему системному администратору.";
            }
        }
        #endregion
    }
}
