// --------------------------------------------------------------------------------------------------------------------
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
            if (filterContext == null)
            {
                return;
            }

            var exception = filterContext.Exception ?? new Exception("No further information");
            this.log.DebugException("EXCEPTION", exception);

            filterContext.ExceptionHandled = true;

            string controllerName = filterContext.RouteData.Values["Controller"] as string ?? string.Empty;
            string actionName = filterContext.RouteData.Values["Action"] as string ?? string.Empty;

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary(new HandleErrorInfo(exception, controllerName, actionName))
            };    
        }
    }
}