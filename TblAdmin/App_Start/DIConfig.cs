using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TblAdmin.Areas.Books.Models;
using TblAdmin.DAL;

namespace TblAdmin
{
    public class DIConfig
    {
        public static void Register()
        {
            // Create a container builder and use it to register all controllers in the assembly.
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register all the constructor params we want injected throughout our app.
            builder.RegisterType<TblAdminContext>().As<TblAdminContext>();

            // Register Model Binders
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // Enable the http abstractions
            builder.RegisterModule(new AutofacWebTypesModule());

            // Everything is registered in the container, so build it, then tell the asp.Net MVC dependancy resolver to 
            // resolve all dependancies from the container we just built.
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}