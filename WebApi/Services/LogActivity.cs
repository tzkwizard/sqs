using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebApi.Models;

namespace WebApi.Services
{
    public class LogActivity
    {
        public static void LogofPost(Contact contact, HttpRequestMessage request)
        {
            string[] endpoint = { Program.SqsUSwestEndpoint, Program.SqsUSwest2Endpoint, Program.SqsUSeastEndpoint };
            Sqsservice sqs = new Sqsservice();
            // string res=sqs.ListAllQueue(endpoint);
            ActivityLog mess = new ActivityLog(((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress, DateTime.Now.ToString(CultureInfo.CurrentCulture),
                  contact.Id);
            sqs.SendtoQueue(Program.SqsUSeastEndpoint, "api", mess);
        }
    }
}