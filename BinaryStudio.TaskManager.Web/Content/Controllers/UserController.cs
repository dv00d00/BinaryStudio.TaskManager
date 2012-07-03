using System;
using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Content.Controllers
{
    public class UserController : Controller
    {
        private UserRepository userRepository;
        //
        // GET: /User/Register
        public UserController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterNewUserModel model)
        {
            if (ModelState.IsValid)
            {
                //createStatus = this.userRepository.RegisterNewUser(model);
                //if (createStatus == MembershipCreateStatus.Success)
                //{
                //    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                //    //TODO: redirect to view with relation employee with account
                //    return RedirectToAction("ConnectUserWithEmployee", "User");
                //}
                //else
                //{
                //    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                //}
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


        public ActionResult ConnectUserWithEmployee()
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