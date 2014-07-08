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
        // Strongly typed redirect
        //
        // SO, INSTEAD OF:
        //          Object routeValues = new { SearchString = "", SortCol = "name", SortColOrder = "asc", Page = "1", PageSize = "3" };
        //          return RedirectToAction("Index", "Books", routeValues);
        // USE: 
        //      1)  return this.RedirectToAction<BooksController>(c => c.Index());   // index action has no parameters
        //      2)  return this.RedirectToAction<BooksController>(c => c.Index(id)); // parameter of type int
        //      3)  return this.RedirectToAction<BooksController>(c => c.Index(s));  // parameter of type string
        //
        // OR FOR VIEW MODEL OBJECTS:
        //      4)  return this.RedirectToAction<BooksController>(c => c.Index(null), new IndexViewModel());
        //
        // OR FOR GENERAL ROUTE VALUES IN QUERY STRING:
        //      5)  Object routeValues = new { SearchString = "", SortCol = "name", SortColOrder = "asc", Page = "1", PageSize = "3" };
        //          return this.RedirectToAction<BooksController>(c => c.Index(null), routeValues);

        // Inspired by https://github.com/uglybugger/MvcNavigationHelpers

        private string extractControllerName<TController>()
        {
            return typeof(TController).Name.Replace("Controller", String.Empty);
        }
        private string extractActionName<TController>(Expression<Action<TController>>  action)
        {
            return ((MethodCallExpression)action.Body).Method.Name;
        }

        // RedirectToActionFor()

        protected ActionResult RedirectToActionFor<TController>(Expression<Action<TController>> action, Object routeValues = null) where TController : Controller
        {
            string controllerName = extractControllerName<TController>();
            string actionName = extractActionName<TController>(action);

            return RedirectToAction(actionName, controllerName, routeValues);
        }

        // Media
        protected void serveZipFile(string zipFilePath, string fileNameWithType)
        {
            Response.ContentType = "application/zip";
            Response.WriteFile(zipFilePath);
            Response.AddHeader("content-disposition", "fileName=" + fileNameWithType);
        }
        
    }
}
