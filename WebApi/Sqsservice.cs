using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amazon.DynamoDBv2;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.First.DynamoDB;
using WebApi.Models;
using Program = WebApi.Program;
using TableOperations = WebApi.DynamoDB.TableOperations;

namespace WebApi
{
    class Sqsservice
    {
        volatile int[] s=new int[2];
        
      
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


        public string ListAllQueue(string[] endpoint)
        {
            string res = "";
            foreach (var t in endpoint)
            {
               res=res+ ListQueue(t)+".  ";
            }
            return res;
        }



        public string SendtoQueue(string endpoint, string queuename,ActivityLog mess)
        {

            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = endpoint + Program.MyAccountNumber + queuename,
                MessageBody = mess.IpAddress+mess.Time+mess.Desciption

            };
            sqs.SendMessage(sendMessageRequest);
            return "sent succeed";
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
            Console.WriteLine("Create a queue called " + queueName + ".  :");
            var sqsRequest = new CreateQueueRequest { QueueName = queuename };
            var createQueueResponse = sqs.CreateQueue(sqsRequest);
            string myQueueUrl = createQueueResponse.QueueUrl;
            Console.WriteLine("Queue:" + myQueueUrl + ".  ");
        }

        //check exist queue

        public string ListQueue(string endpoint)
        {
            AmazonSQSConfig amazonSqsConfig = new AmazonSQSConfig { ServiceURL = endpoint };
            AmazonSQSClient sqs = new AmazonSQSClient(amazonSqsConfig);

            string res = "";

            //Confirming the queue exists
            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueues(listQueuesRequest);
            
            res=res+"Printing list of Amazon SQS queues.  : ";
            if (listQueuesResponse.QueueUrls != null)
            {
                foreach (String queueUrl in listQueuesResponse.QueueUrls)
                {
                    res = res + queueUrl+".  ";
                }
            }
            return res;
        }

        // Receiving a message
        public void ReceviveMessage(string endpoint, string queuename)
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
                Console.WriteLine("Printing received message from \n" + endpoint + Program.MyAccountNumber + Program.Queuename);
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


                       TableOperations.PutItem(client, s, endpoint,message.MessageId);
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
            // Console.ReadLine();
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
    }
}