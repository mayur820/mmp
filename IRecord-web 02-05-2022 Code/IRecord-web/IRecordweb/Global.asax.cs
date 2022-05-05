using IRecordweb.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IRecordweb
    {
    public class MvcApplication : System.Web.HttpApplication
        {
        protected void Application_Start()
            {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Calling Global action filter

            GlobalFilters.Filters.Add(new CustomExceptionFilter());
        }

        }
    }
