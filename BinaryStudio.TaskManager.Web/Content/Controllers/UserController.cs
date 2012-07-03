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
                    ModelState.AddModelError("", "������� ������� ������ ��� ����� ������ � ��������.");
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
                    return "������������ � ����� ������ ��� ����������. ����������, ������� ������ ��� ������������.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "������������ � ����� e-mail ��� ����������. ����������, ������� ������ e-mail.";

                case MembershipCreateStatus.InvalidPassword:
                    return "�������� ������. ����������, ������� ���������� �������� ������.";

                case MembershipCreateStatus.InvalidEmail:
                    return "������ �������������� e-mail. ����������, ��������� ��������� �������� � ��������� �������.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "����������� ����� ��� �������������� ������ ��������. ����������, ��������� ��������� �������� � ��������� �������.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "����������� ������ ��� �������������� ������ ��������. ����������, ��������� ��������� �������� � ��������� �������.";

                case MembershipCreateStatus.InvalidUserName:
                    return "�������� ��� ������������. ����������, ��������� ��������� �������� � ��������� �������.";

                case MembershipCreateStatus.ProviderError:
                    return "�������� ����������� ������������ ��������� �� ������. ����������, ��������� ���� ������ � ���������� ��� ���. ���� �������� �� �����������, ���������� � ������ ���������� ��������������.";

                case MembershipCreateStatus.UserRejected:
                    return "������ � �������� ������������ ��� �������. ����������, ��������� ���� ������ � ���������� ��� ���. ���� �������� �� �����������, ���������� � ������ ���������� ��������������.";

                default:
                    return "�������� ����������� ������. ����������, ��������� ���� ������ � ���������� ��� ���. ���� �������� �� �����������, ���������� � ������ ���������� ��������������.";
            }
        }

        #endregion
    }

}