using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using WebApi.Models;

namespace WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

  
    public class WebApiApplication : System.Web.HttpApplication
    {
        public const string Userlog = "Userlog";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

           // WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // Application["Userlog"] = Userlog;
        }



      /*  void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // Session is Available here
            HttpContext context = HttpContext.Current;
            context.Session["foo"] = "foo";
        }*/

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
           

        }

        
    }
}