﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.App_Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
         ModelFactory _modelFactory;
        ICountingKsRepository _repo;

        public BaseApiController(ICountingKsRepository repo)
        {
            _repo = repo;
            
        }

        protected ICountingKsRepository TheRepository
        {
            get { return _repo; }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory=new ModelFactory(this.Request);
                }
                return _modelFactory;
            }
        }
    }
}