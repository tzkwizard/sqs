using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using WebApi.Models;


namespace WebApi.Controllers
{
    
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public static string GetClientIpAddress(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                return ((RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name]).Address;

            return "IP Address Unavailable";    //here the user can return whatever they like
        }
        // GET api/values/5
        public string Get(int id)
        {
            string[] endpoint = { Program.SqsUSwestEndpoint, Program.SqsUSwest2Endpoint, Program.SqsUSeastEndpoint };
            Sqsservice sqs = new Sqsservice();
           // string res=sqs.ListAllQueue(endpoint);
          ActivityLog mess=new ActivityLog(GetClientIpAddress(Request), DateTime.Now.ToString(CultureInfo.CurrentCulture),
                "first commit");
            
            
           string res= sqs.SendtoQueue(Program.SqsUSeastEndpoint, "api", mess);
           // HttpRequestMessage request = base.Request;
            return res ; 
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}