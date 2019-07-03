using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using MongoDB.Driver;
using DroneWebConsole.Controllers;

namespace DroneWebConsole.Controllers
{
    public class AdminRequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var sess = filterContext.HttpContext.Session.GetString("email");
                if (sess == null || sess == "")
                {
                    filterContext.Result = new RedirectResult("/Auth/Login");
                }
            }
            catch (Exception e)
            {
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


       
    }
}
