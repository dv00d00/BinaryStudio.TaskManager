using System.Data.Entity;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    using NLog;

    using Ninject;
    using Ninject.Web.Common;

    public class MvcApplication : NinjectHttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler());
        }
        public class ErrorHandler: HandleErrorAttribute
        {

            private readonly Logger log = LogManager.GetCurrentClassLogger();
            public override void OnException(ExceptionContext filterContext)
            {
                if (filterContext == null) return;

                var ex = filterContext.Exception ?? new Exception("No further information");
                this.log.DebugException("EXCEPTION", ex);

                filterContext.ExceptionHandled = true;
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "HumanTasks",
                    action = "AllManagersWithTasks",
                    id = UrlParameter.Optional
                });
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new DatabaseInitializer());
        }
    }
}