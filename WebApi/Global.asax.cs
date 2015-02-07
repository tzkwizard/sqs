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

namespace WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

  
    public class WebApiApplication : System.Web.HttpApplication
    {
        private Type _workerType;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

           // WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Console.WriteLine("hh");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = Context;
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            HttpWorkerRequest worker = ((IServiceProvider) context).GetService(_workerType) as HttpWorkerRequest;
            if (worker != null)
            {
                string key = worker.GetRawUrl();
            }
            HttpRequestMessage requestMessage = context.Items["MS_HttpRequestMessage"] as HttpRequestMessage;


            if (!request.RequestType.Equals("GET"))
            {
                Queuestore q = new Queuestore();

               // Task.Run(() =>(q.SendQueueAsyncAll(100,  request)));
                q.SendQueueAsyncAll(100, request);
            }
        
         }
    }
}