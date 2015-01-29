using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.App_Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class DiariresController : BaseApiController
    {
        private ICountingKsIdentityService _identityService;

        public DiariresController(ICountingKsRepository repo,ICountingKsIdentityService identityService):base(repo)
        {
         _identityService  = identityService;
        }


        public IEnumerable<DiaryModel> Get()
        {
            var username = _identityService.CurrentUser;
            var res = TheRepository.GetDiaries(username)
                .OrderByDescending(f => f.CurrentDate)
                .Take(10)
                .ToList()
                .Select(f => TheModelFactory.Create(f));
            return res;
        }
    }
}
