using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

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
            Console.WriteLine(message);
            // Create or reference an existing queue 
            //  CloudQueue queue = q.CreateQueueAsync(queuename).Result;

           // q.Listqueue();
           // q.ProcessMessageAsync("qqwwss").Wait();
            //  q.SendQueueAsync(15).Wait();
            // q.SendQueueAsyncAll(250);
            // Task.Run(() => q.ProcessMessageAsync("qqwwss"));
           // q.ProcessMessage("qqwwss");
                    

           //Task t = q.ProcessAsync("qqwwss");

             //q.ProcessMessageAsync("qqwwss");
            //Task.Run(() => q.ProcessAsync("qqwwss"));

             var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
             // Create a queue client for interacting with the queue service
             CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

             CloudQueue queue = queueClient.GetQueueReference("qqwwss");
             q.InsertData(message);
             q.ProcessMessageAsync(queue);

        }
    }
}
