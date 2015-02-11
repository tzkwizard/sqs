using System;
using System.Globalization;
using System.IO;
using System.Web;
using WebApi.Models;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Providers.Entities;
using System.Web.Routing;

namespace WebApi
{
    public class MyModule : IHttpModule
    {
        public const string Userlog = "Userlog";

        public MyModule()
        {
        }

        public void Init(HttpApplication objApplication)
        {
// Register event handler of the pipe line
            objApplication.BeginRequest += new EventHandler(this.context_BeginRequest);
            objApplication.EndRequest += new EventHandler(this.context_EndRequest);
            
        }

        public void Dispose()
        {
        }

        public void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpResponse response = context.Response;
            string s = response.Status;
            Queuestore q = new Queuestore();


            q.SendQueueAsyncAll(1, s);
        }

        public void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            string ipAddress = context.Request.UserHostAddress;
            string requestHeader=context.Request.Headers.ToString(); 

            ELSLog esLog = new ELSLog();
            esLog.ElsIpaddress = ipAddress;
            esLog.ElsRequest = "["+DateTime.Now.ToString("dd/MMM/yyyy:HH:mm:ss zz")+"]" +" \""+ request.HttpMethod+" " 
                + request.Path +"\" "+ response.StatusCode+" 2 "+"\""+ request.UrlReferrer+"\" " + "\""+request.UserAgent+"\"";

        /*    if (HttpContext.Current.Session != null)
            {
               // context.Session[Userlog] = esLog;
                Session[Userlog] = esLog;
            }*/
            context.Items[Userlog] = esLog;



            if (!request.RequestType.Equals("GET"))
            {
                Queuestore q = new Queuestore();

            }
        }
    }
}