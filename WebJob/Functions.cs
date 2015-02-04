﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorageQueue;
using Microsoft.Azure.WebJobs;

namespace WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("qqwwss")] string message, TextWriter log)
        {
            log.WriteLine(message);
            Queuestore q = new Queuestore();
            // Create or reference an existing queue 
            //  CloudQueue queue = q.CreateQueueAsync(queuename).Result;

           // q.Listqueue();

            //q.SendQueueAsync(15).Wait();
            // q.SendQueueAsyncAll(250);
            q.ProcessMessageAsync("qqwwss").Wait();
            //q.ProcessMessageAsyncAll("qqwwss");


        }
    }
}
