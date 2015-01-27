using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api.Models;
using Microsoft.Ajax.Utilities;

namespace Api.Services
{
    public class ApiRepository
    {
        
        public Product[] GetAllFoods()
        {
            return new Product[]
        {
             new Product
             {
                  Id = 1,
                  Name = "apple",
                  Category="fruit",
                  Price = (decimal) 2.3
             },
             
             new Product
             {
                  Id = 2,
                  Name = "pinapple",
                  Category="fruit",
                  Price = (decimal) 3.3
             },
             new Product
             {
                  Id = 3,
                  Name = "beef",
                  Category="meat",
                  Price = (decimal) 5.3
             },
             new Product
             {
                  Id = 4,
                  Name = "pork",
                  Category="meat",
                  Price = (decimal) 4.3
             },
             new Product
             {
                  Id = 5,
                  Name = "coke",
                  Category="drinking",
                  Price = (decimal) 1.3
             },
        };
        }
    }
}