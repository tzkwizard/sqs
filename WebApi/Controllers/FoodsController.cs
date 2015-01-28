﻿using System;
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
    public class FoodsController : BaseApiController
    {

        public FoodsController(ICountingKsRepository repo):base(repo)
        {
            
        }
        public IEnumerable<FoodModel> Get(bool includeMeasures=true)
        {
            IQueryable<Food> query;

            query = includeMeasures ? TheRepository.GetAllFoodsWithMeasures() : TheRepository.GetAllFoods();


            var res = query.OrderBy(f => f.Id)
                .Take(25)
                .ToList()
                .Select(f => TheModelFactory.Create(f));


            return res;
        }

        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheRepository.GetFood(foodid));
        }
       
    }
   
}
