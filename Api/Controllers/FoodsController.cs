using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Api.Models;
using Api.Services;

namespace Api.Controllers
{
    public class FoodsController : ApiController
    {
       
        public IEnumerable<Product> Get()
        {
            var repo = new ApiRepository();
            var result = repo.GetAllFoods()
                .OrderBy(f => f.Id)
                .Take(5)
                .ToList();
            return result;
        }
    }

    public interface IApiRepository
    {
    }
}
