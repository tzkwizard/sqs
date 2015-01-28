using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.ECS.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.First.DynamoDB;
using Task = System.Threading.Tasks.Task;

namespace SQS.First
{
    class Sqsservice
    {
        volatile int[] s = new int[2];


        public void RecevieAllMessage(string[] endpoint, int threadnumber)
        {
            s[0] = 0;
            foreach (var t in endpoint)
            {
                RecevieMessage2(threadnumber, t, Program.Queuename);
            }
        }


        public class ThreadState
        {
            public ThreadState(string info, string endpoint, int number, string queuename)
            {
                this.Info = info;
                this.Endpoint = endpoint;
                this.Number = number;
                this.Name = queuename;
            }
            public string Info { get; set; }
            public string Endpoint { get; set; }
            public int Number { get; set; }
            public string Name { get; set; }
        }


        public void ListAllQueue(string[] endpoint)
        {

            foreach (var t in endpoint)
            {
                ListQueue(t);
            }
        }


        //Sending a message

        public void SendMessage(string endpoint, int number, string queuename)
        {
            // var sqs = AWSClientFactory.CreateAmazonSQSClient();
            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);

            Console.WriteLine("Sending message to" + endpoint + Program.MyAccountNumber + queuename + ".\n");
            for (var i = 0; i < number; i++)
            {
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = endpoint + Program.MyAccountNumber + queuename,
                    MessageBody = "This is my message text." + i

                };
                sqs.SendMessage(sendMessageRequest);
                Console.WriteLine("Message " + i + "has been sent");
            }

        }

        //  Creating a queue

        public void CreateQueue(AmazonSQSClient sqs, string queuename)
        {
            // var sqs = AWSClientFactory.CreateAmazonSQSClient();
            var queueName = queuename;
            Console.WriteLine("Create a queue called " + queueName + ".\n");
            var sqsRequest = new CreateQueueRequest { QueueName = queuename };
            var createQueueResponse = sqs.CreateQueue(sqsRequest);
            string myQueueUrl = createQueueResponse.QueueUrl;
            Console.WriteLine("Queue:" + myQueueUrl + ".\n");
        }

        //check exist queue

        public void ListQueue(string endpoint)
        {
            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);

            //Confirming the queue exists
            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueues(listQueuesRequest);


            Console.WriteLine("Printing list of Amazon SQS queues.\n");
            if (listQueuesResponse.QueueUrls != null)
            {
                foreach (String queueUrl in listQueuesResponse.QueueUrls)
                {
                    Console.WriteLine("  QueueUrl: {0}", queueUrl);
                }
            }

        }

        public string[] ListQueuename(string endpoint)
        {
            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);
            int s = endpoint.Length + Program.MyAccountNumber.Length;

            int l = 0;
            //Confirming the queue exists
            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueues(listQueuesRequest);
            int size = listQueuesResponse.QueueUrls.Count;
            string[] queueCollection = new string[size];
            //Console.WriteLine("Printing list of Amazon SQS queues.\n");
            if (listQueuesResponse.QueueUrls != null)
            {
                int i = 0;
                foreach (String queueUrl in listQueuesResponse.QueueUrls)
                {
                    l = queueUrl.Length;
                    queueCollection[i] = queueUrl.Substring(s, l - s);
                    i++;
                }
            }

            return queueCollection;
        }



        // Receiving a message
        public DateTime ReceviveMessage(string endpoint, string queuename)
        {
            //var sqs = AWSClientFactory.CreateAmazonSQSClient();
            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);


            var receiveMessageRequest = new ReceiveMessageRequest
            {
                AttributeNames = new List<string>() { "All" },
                MaxNumberOfMessages = 10,
                QueueUrl = endpoint + Program.MyAccountNumber + Program.Queuename,
                VisibilityTimeout = (int)TimeSpan.FromMinutes(2).TotalSeconds,
                WaitTimeSeconds = (int)TimeSpan.FromSeconds(5).TotalSeconds
            };

            // var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = USwest2Url };
            var receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);
            if (receiveMessageResponse.Messages.Count > 0)
            {
                // Console.WriteLine("Printing received message from \n" + endpoint + Program.MyAccountNumber + Program.Queuename);
                foreach (var message in receiveMessageResponse.Messages)
                {

                    //                    if (!string.IsNullOrEmpty(message.MessageId))
                    //                    {
                    //                        Console.WriteLine("    MessageId: {0}", message.MessageId);
                    //                    }
                    //                    if (!string.IsNullOrEmpty(message.ReceiptHandle))
                    //                    {
                    //                        Console.WriteLine("    ReceiptHandle: {0}", message.ReceiptHandle);
                    //                    }
                    //                    if (!string.IsNullOrEmpty(message.MD5OfBody))
                    //                    {
                    //                        Console.WriteLine("    MD5OfBody: {0}", message.MD5OfBody);
                    //                    }
                    //                    if (!string.IsNullOrEmpty(message.Body))
                    //                    {
                    //                        Console.WriteLine("    Body: {0}", message.Body);
                    //                    }


                    if (message.Body.Count() > 5)
                    {
                        //Console.WriteLine("Setting up DynamoDB client");

                        AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
                        config.ServiceURL = Program.DynamoDbUSwest2Endpoint;
                        var client = new AmazonDynamoDBClient(config);


                        TableOperations.PutItem(client, s, endpoint, message.MessageId);
                        // TableOperations.PutItem2(5,client, s, endpoint);
                        s[0]++;
                    }
                    else
                    {
                        Console.WriteLine("nothing sent ");
                    }

                    //           
                    //                    foreach (string attributeKey in message.Attributes.Keys)
                    //                    {
                    //                        Console.WriteLine("  Attribute");
                    //                        Console.WriteLine("    Name: {0}", attributeKey);
                    //                        var value = message.Attributes[attributeKey];
                    //                        Console.WriteLine("    Value: {0}", string.IsNullOrEmpty(value) ? "(no value)" : value);
                    //                    }

                    // var messageRecieptHandle = receiveMessageResponse.Messages[0].ReceiptHandle;
                    //                                        var messageRecieptHandle = message.ReceiptHandle;
                    //                    
                    //                                        //Deleting a message
                    //                                        DateTime t = DateTime.Now;
                    //                                        Console.WriteLine("Deleting the message from queue.\n"+t);
                    //                                        var deleteRequest = new DeleteMessageRequest
                    //                                        {
                    //                                           QueueUrl = endpoint +Program.MyAccountNumber+ Program.Queuename,
                    //                                            ReceiptHandle = messageRecieptHandle
                    //                                        };
                    //                                        sqs.DeleteMessage(deleteRequest);
                }

            }
            else
            {
                Console.WriteLine("No messages received.");
            }

            // Console.WriteLine(DateTime.Now);
            return DateTime.Now;
        }





        // Receiving endpoint all message
        public DateTime ReceviveEndpointAllQueueMessage(string endpoint, string[] queueCollection)
        {
            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);
            foreach (var queuename in queueCollection)
            {

                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    AttributeNames = new List<string>() { "All" },
                    MaxNumberOfMessages = 10,
                    QueueUrl = endpoint + Program.MyAccountNumber + queuename,
                    VisibilityTimeout = (int)TimeSpan.FromMinutes(2).TotalSeconds,
                    WaitTimeSeconds = (int)TimeSpan.FromSeconds(5).TotalSeconds
                };

                // var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = USwest2Url };
                var receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);
                if (receiveMessageResponse.Messages.Count > 0)
                {
                    foreach (var message in receiveMessageResponse.Messages)
                    {

                        if (message.Body.Count() > 5)
                        {
                            //Console.WriteLine("Setting up DynamoDB client");                      
                            AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
                            config.ServiceURL = Program.DynamoDbUSwestEndpoint;
                            var client = new AmazonDynamoDBClient(config);
                            TableOperations.PutItem(client, s, endpoint, message.MessageId);
                            // TableOperations.PutItem2(5,client, s, endpoint);
                            //s[0]++;
                        }
                        else
                        {
                            Console.WriteLine("nothing sent ");
                        }
                        // var messageRecieptHandle = receiveMessageResponse.Messages[0].ReceiptHandle;
                        var messageRecieptHandle = message.ReceiptHandle;
                        //Deleting a message
                        DateTime t = DateTime.Now;
                        Console.WriteLine("Deleting the message from queue.\n" + t);
                        var deleteRequest = new DeleteMessageRequest
                        {
                            QueueUrl = endpoint + Program.MyAccountNumber + Program.Queuename,
                            ReceiptHandle = messageRecieptHandle
                        };
                        sqs.DeleteMessage(deleteRequest);

                    }

                }
                else
                {
                    Console.WriteLine("No messages received.");
                }
                Console.WriteLine(queuename + "is finished");
            }
            return DateTime.Now;
        }

        //Sending message use multiple thread

        public void SendMessage2(int numberThread, string endpoint, int number, string queuename)
        {
            System.Threading.WaitCallback waitCallback = new WaitCallback(SendMessageThreadWork);
            ThreadState threadstate = new ThreadState("", endpoint, number, queuename);
            for (int i = 0; i < numberThread; i++)
            {

                ThreadPool.QueueUserWorkItem(waitCallback, threadstate);
            }
            Console.ReadLine();
        }

        //Sending Thread

        public void SendMessageThreadWork(object state)
        {
            Console.WriteLine("thread start…… {0}", Thread.CurrentThread.ManagedThreadId);

            int number = ((ThreadState)state).Number;
            string endpoint = ((ThreadState)state).Endpoint;
            string queuename = ((ThreadState)state).Name;
            SendMessage(endpoint, number, queuename);

            Thread.Sleep(4000);
            Console.WriteLine("thread end…… {0}", Thread.CurrentThread.ManagedThreadId);
            //((ThreadState)state).Info++;
        }

        //Receive message use multiple thread
        public void RecevieMessage2(int threadNumber, string endpoint, string queuename)
        {
            WaitCallback waitCallback = new WaitCallback(RecevieMessageThreadWork);
            ThreadState threadstate = new ThreadState("", endpoint, 4, queuename);
            for (int i = 0; i < threadNumber; i++)
            { ThreadPool.QueueUserWorkItem(waitCallback, threadstate); }
            // Program.stop.Stop();
            // Console.WriteLine("gaga");
        }

        //Receiving Thread
        public void RecevieMessageThreadWork(object state)
        {
            // Console.WriteLine("thread start…… {0}", Thread.CurrentThread.ManagedThreadId);
            string endpoint = ((ThreadState)state).Endpoint;
            string queuename = ((ThreadState)state).Name;
            ReceviveMessage(endpoint, queuename);
            Thread.Sleep(2000);
            //  Console.WriteLine("thread end…… {0}", Thread.CurrentThread.ManagedThreadId); 
        }

        public async Task<DateTime> AsyncProessorForEndpoint(string endpoint, string[] queueanmeCollection)
        {

            DateTime res = await Task.Run(() => ReceviveEndpointAllQueueMessage(endpoint, queueanmeCollection));
            return res;
        }


        public DateTime AsyncSqsProessor(string[] endpointCollection)
        {
            DateTime res = new DateTime();
            foreach (var endpoint in endpointCollection)
            {
                string[] queueNameCollection = ListQueuename(endpoint);
                Task<DateTime> task = AsyncProessorForEndpoint(endpoint, queueNameCollection);
                for (int i = 0; i < 5; i++)
                {
                    queueNameCollection = ListQueuename(endpoint);
                    task = AsyncProessorForEndpoint(endpoint, queueNameCollection);
                }
                res = task.Result;
                Console.WriteLine("Delay 5s");
                Thread.Sleep(5000);
            }


            return res;
        }

    }
}