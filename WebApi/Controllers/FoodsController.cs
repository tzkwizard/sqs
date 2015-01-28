using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.App_Data;
using WebApi.App_Data.Entities;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class FoodsController : ApiController
    {
        private ModelFactory _modelFactory;
        private ICountingKsRepository _repo;

        public FoodsController(ICountingKsRepository repo)
        {
            _repo = repo;
            _modelFactory = new ModelFactory();
        }
        public IEnumerable<FoodModel> Get(bool includeMeasures=true)
        {
            IQueryable<Food> query;

            query = includeMeasures ? _repo.GetAllFoodsWithMeasures() : _repo.GetAllFoods();


            var res = query.OrderBy(f => f.Id)
                .Take(25)
                .ToList()
                .Select(f => _modelFactory.Create(f));


            return res;
        }

        public FoodModel Get(int foodid)
        {
            return _modelFactory.Create(_repo.GetFood(foodid));
        }
       
    }
   
}
