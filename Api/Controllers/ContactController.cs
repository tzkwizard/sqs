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
    public class ContactController : ApiController
    {
        private readonly ContactRepository _contactRepository;

        public ContactController()
        {
            this._contactRepository = new ContactRepository();
        } 
        public Contact[] Get()
        {
            return _contactRepository.GetAllContacts(); 
        }
    }
}
