﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using RavenDBApplication.Controllers;

namespace RavenDBApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest +=
                (sender, args) =>
                    {
                        HttpContext.Current.Items[RavenController.CurrentRequestDocumentSessionKey] =
                            RavenController.DocumentStore.OpenSession();
                    };
            EndRequest +=
                (sender, args) =>
                    {
                        using (var session = (IDocumentSession) HttpContext.Current.Items[RavenController.CurrentRequestDocumentSessionKey])
                        {
                            if (session == null)
                                return;
                            if (Server.GetLastError() != null)
                                return;
                            session.SaveChanges();
                        }
                    };
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitializeDocumentStore();
        }

        private void InitializeDocumentStore()
        {
            var documentStore = new DocumentStore
                                    {
                                        Url = "http://localhost:8080",
                                        DefaultDatabase = "QuickStart"
                                    };
            documentStore.Initialize();
            RavenController.DocumentStore = documentStore;
        }
    }
}