﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;        
        private readonly IUserProcessor userProcessor;
        public AccountController(IUserRepository userRepository, IUserProcessor userProcessor)
        {
            this.userRepository = userRepository;
            this.userProcessor = userProcessor;
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
                if (userProcessor.LogOnUser(model.UserName, model.Password))
                {
                    userProcessor.SetRoleToUserFromDB(model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("AllManagersWithTasks", "HumanTasks");
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

            return RedirectToAction("LogOn", "Account");
        }

       
    }
}
