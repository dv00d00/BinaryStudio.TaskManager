﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorHandler.cs" company="">
//   
// </copyright>
// <summary>
//   Global Exception Handler, logs all unhandled Exceptions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web
{
    using System;
    using System.Web.Mvc;

    using NLog;

    /// <summary>
    /// Global Exception Handler, logs all unhandled Exceptions 
    /// </summary>
    public class ErrorHandler : HandleErrorAttribute
    {        
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null) return;

            var ex = filterContext.Exception ?? new Exception("No further information");
            this.log.DebugException("EXCEPTION", ex);

            filterContext.ExceptionHandled = true; 

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };    
        }
    }
}