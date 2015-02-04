using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DataStorageQueue;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
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

        public HttpResponseMessage Post(Contact contact, HttpRequestMessage request)
        {
            _contactRepository.SaveContact(contact);

            var response = Request.CreateResponse<Contact>(System.Net.HttpStatusCode.Created, contact);
           

            Queuestore q = new Queuestore();
            
            string queuename = "hahaha";
            // Create or reference an existing queue 
           // CloudQueue queue = q.CreateQueueAsync(queuename).Result;
         
            int num = Convert.ToInt32(contact.Number);
           
            q.SendQueueAsyncAll(num,request);
            return response;
        }


        
    }


}
