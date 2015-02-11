using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ELSLog
    {
        public string ElsRequest { get; set; }
        public string ElsResponse { get; set; }
        public string ElsIpaddress { get; set; }
        public string ElsUserInfromation { get; set; }
    }
}