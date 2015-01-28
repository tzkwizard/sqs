using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.App_Data;

namespace WebApi.Controllers
{
    public class FoodsController : ApiController
    {
        public object Get()
        {
            var repo = new CountingKsRepository(new CountingKsContext());

            var res = repo.GetAllFoods()
                .OrderBy(f => f.Description)
                .Take(25)
                .ToList();

            return res;
        }
    }
}
