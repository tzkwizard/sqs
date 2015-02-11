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
using StackExchange.Redis;


namespace WebJob
{
    public class Functions
    {


      /*  public static void Redis([QueueTrigger("qqwwss")] string message, TextWriter log)
        {
            
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("avmredis.redis.cache.windows.net,password=L+XyW+nDc0GbQcHaC8g8jpW8ACUuwYcF4wTQmQnsbRo=,allowAdmin=true");
            IDatabase cache = connection.GetDatabase();

            // Perform cache operations using the cache object...
            // Simple put of integral data types into the cache

          /*  for (int i = 1; i < 20; i++)
            {
                cache.StringSet(i.ToString(), "value"+i);
            }#1#
            //cache.StringSet("key2", 25);
           var server = connection.GetServer("avmredis.redis.cache.windows.net:6379");
            server.FlushDatabase();

           /*for(int i=1;i<30;i++)
            {cache.ListLeftPush("s", i);}#1#

         string tem="::1 - - [10/Feb/2016:15:49:12 -06] "+"\"POST /api/contact\""+" 200 2 "+"\"http://localhost:59524/\""+
            " \"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.111 Safari/537.36\"";
           cache.ListLeftPush("s", tem);
            foreach (var s in cache.ListRange("s"))
            {
                Console.WriteLine(s);
            }
            

           // cache.SetAdd("2", );
          // string tmp=cache.ListLeftPop("s");
           /* long x=cache.ListLength("s");
            Console.WriteLine(x);#1#

            /*foreach (var s in connection.GetEndPoints())
            {
                Console.WriteLine(s);
            }
            // Simple get of data types from the cache
            foreach (var i in server.Keys())
            {
                
                
                string key1 = cache.StringGet(i);
                int key2 = (int) cache.StringGet("key2");
                Console.WriteLine(key1 + "" + key2);
            }#1#
        }*/
     

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
