﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Services
{
   
    public class ContactRepository
    {
        private const string CacheKey = "ContactStore";


        public ContactRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var contacts = new Contact[]
                {
                new Contact
                {
                    Number = "1", Name = "aotuo zhang"
                }
               
            };

                    ctx.Cache[CacheKey] = contacts;
                }
            }
        }

        public Contact[] GetAllContacts()
        {
            /*return new Contact[]
        {
             new Contact
             {
                  Id = 1,
                  Name = "Glenn Block"
             },
             new Contact
             {
                  Id = 2,
                  Name = "Dan Roth"
             }
        };*/


            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Contact[])ctx.Cache[CacheKey];
            }

            return new Contact[]
        {
            new Contact
            {
                Number = "0",
                Name = "Placeholder"
            }
        };
        }



        public bool SaveContact(Contact contact)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Contact[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(contact);
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}