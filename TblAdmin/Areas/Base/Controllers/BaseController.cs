using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;

namespace TblAdmin.Areas.Base.Controllers
{
    public abstract class BaseController : Controller
    {
        // Add some good ideas from Matt Honeycutt: http://blog.pluralsight.com/what-is-application-framework
        
        [Obsolete("Do not use string based redirects. Please use the strongly typed version instead in the BaseController.")]
        protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            throw (new InvalidOperationException("Do not use string based redirects. Please use the strongly typed version instead in the BaseController."));
        }

        [Obsolete("Do not use string based redirects. Please use the strongly typed version instead in the BaseController.")]
        protected new RedirectToRouteResult RedirectToAction(string actionName, string controllerName)
        {
            throw (new InvalidOperationException("Do not use string based redirects. Please use the strongly typed version instead in the BaseController."));
        }
        
        // You can mask/hide other overloads of RedirectToAction as well, here.

        // This is the strongly typed version you should use in your controllers
        // Usage: return this.RedirectToAction<BooksController>(c => c.Index());
        // Instead of: return RedirectToAction("Index", "Books");
        //protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action) where TController : Controller
        //{
        //    return ControllerExtensions.RedirectToAction(this, action);
        //}
    }
}
