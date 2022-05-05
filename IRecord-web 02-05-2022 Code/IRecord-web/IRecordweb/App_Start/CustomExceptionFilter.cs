using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.App_Start
{
    public class CustomExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        //public void OnException(ExceptionContext filterContext)
        //{
        //    if (!filterContext.ExceptionHandled && filterContext.Exception is NullReferenceException)
        //    {
        //        filterContext.Result = new RedirectResult("customErrorPage.html");
        //        filterContext.ExceptionHandled = true;
        //    }
        //}
        public void OnException(ExceptionContext filterContext)

        {

            Exception e = filterContext.Exception;
            string ErrorText = e.Message.ToString();
            string LocationError = e.StackTrace.ToString();
            

            filterContext.ExceptionHandled = true;
            HttpContext.Current.Session["ErrorText"] = ErrorText;
            HttpContext.Current.Session["LocationError"] = LocationError;
            filterContext.Result = new ViewResult()
            {

                ViewName = "RunTimeException",
             


            };
     


        }
    }
}