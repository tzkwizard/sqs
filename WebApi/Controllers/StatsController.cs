using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.App_Data;

namespace WebApi.Controllers
{
    public class StatsController : BaseApiController
    {
        public StatsController(ICountingKsRepository repo) : base(repo)
        {
            
        }

        [Route("api/Stats/123")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var result = new
            {
                NumFoods=TheRepository.GetAllFoods().Count(),
                NumUsers=TheRepository.GetApiUsers().Count()
            };
            return Request.CreateResponse(result);
        }
    }
}
