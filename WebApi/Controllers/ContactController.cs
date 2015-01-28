using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
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

        public HttpResponseMessage Post(Contact contact)
        {
            _contactRepository.SaveContact(contact);

            var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);



            LogActivity.LogofPost(contact,Request);    
            return response;
        }


        
    }


}
