using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

            HttpRequestHeaders res=request.Headers;
            string r = res.Accept + "//" + res.AcceptCharset + "//" + res.Connection + "//" + res.Referrer + "//";

          /*  Task<DateTime> task = sqs.AsyncProessor(SqsUSeastEndpoint, Queuename);
            for (int i = 0; i < 50; i++)
            { task = sqs.AsyncProessor(SqsUSeastEndpoint, Queuename); }*/

            ActivityLog mess = new ActivityLog(((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress, DateTime.Now.ToString(CultureInfo.CurrentCulture),
                  r);
            for(int i=0;i<50;i++)
            {Task<DateTime> task = sqs.AsyncSendtoQueue(Program.SqsUSeastEndpoint, "api", mess);}

            //sqs.SendtoQueue(Program.SqsUSeastEndpoint, "api", mess);
        }
    }
}