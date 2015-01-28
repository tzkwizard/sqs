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
    public class MeasuresController : ApiController
    {
        private ICountingKsRepository _repo;
        private ModelFactory _modelFactory;

        public MeasuresController(ICountingKsRepository repo)
        {
            _repo = repo;
            _modelFactory = new ModelFactory();
        }


       
    }
}
