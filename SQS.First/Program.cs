/*******************************************************************************
* Copyright 2009-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CloudWatchLogs.Model;
using Amazon.DirectConnect.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using SQS.First.DynamoDB;


namespace SQS.First
{
   

    
    class Program
    {
       public const string SqsUSwest2Endpoint = "https://sqs.us-west-2.amazonaws.com";
       public const string SqsUSeastEndpoint = "https://sqs.us-east-1.amazonaws.com";
       public const String SqsUSwestEndpoint = "https://sqs.us-west-1.amazonaws.com";
       public const String DynamoDbUSwest2Endpoint = "http://dynamodb.us-west-2.amazonaws.com";
       public const String DynamoDbUSwestEndpoint = "http://dynamodb.us-west-1.amazonaws.com";
         public const string MyAccountNumber2 = "/192607422200/";
         public const string MyAccountNumber = "/606227029357/";
         public const string Queuename = "tzkQueue";
         public const string USwest2Url = SqsUSwest2Endpoint + MyAccountNumber + Queuename;
         public const string USeastUrl = SqsUSeastEndpoint + MyAccountNumber + Queuename;
         public const string USwestUrl = SqsUSwest2Endpoint + MyAccountNumber + Queuename;

        int[] s = new int[3];

        public static Stopwatch stop = new Stopwatch();
        public static void Main(string[] args)
        {
            //s[0] = 1;
            
            string[] endpointCollection = { SqsUSwestEndpoint, SqsUSwest2Endpoint, SqsUSeastEndpoint };
            try
            {
                Console.WriteLine("===========================================");
                Console.WriteLine("Getting Started with Amazon SQS");
                Console.WriteLine("===========================================\n");

                Sqsservice sqs=new Sqsservice();
               //sqs.ListAllQueue(endpoint);
               string[] queueNameCollection = sqs.ListQueuename(SqsUSwestEndpoint);


                //sqs.ReceviveEndpointMessage(SqsUSwest2Endpoint, queueCollection);

                Console.WriteLine(DateTime.Now);
                DateTime start = DateTime.Now;

                DateTime end = sqs.AsyncSqsProessor(endpointCollection);
                Console.WriteLine(end - start);





               /* Task<DateTime> task = sqs.AsyncProessorForEndpoint(SqsUSwestEndpoint, queueNameCollection);
                for (int i = 0; i < 50; i++)
                {
                    queueNameCollection = sqs.ListQueuename(SqsUSwestEndpoint);
                    task = sqs.AsyncProessorForEndpoint(SqsUSwestEndpoint, queueNameCollection);
                }
                Console.WriteLine("async");
                Console.WriteLine(task.Result - start);*/

                /* start = DateTime.Now;
                for(int i=0;i<10;i++)
              { sqs.ReceviveMessage(SqsUSeastEndpoint, Queuename);}
            //    Console.WriteLine(task.ToString()+"gaga");
                DateTime end = DateTime.Now;
                Console.WriteLine("one thread");
               Console.WriteLine(end - start);


                //stop.Start();
                // sqs.SendMessage(SqsUSwestEndpoint, 15, Queuename);

                // sqs.RecevieAllMessage(endpoint,25);

                // CreateQueue(sqs, queuename);

              /*  sqs.SendMessage2(40, SqsUSwestEndpoint, 40, "aoi");
                sqs.SendMessage2(40, SqsUSwestEndpoint, 40, "api");
                sqs.SendMessage2(40, SqsUSeastEndpoint, 40, "api");
                sqs.SendMessage2(40, SqsUSeastEndpoint, 40, "test");
                sqs.SendMessage2(40, SqsUSwest2Endpoint, 40, "test");
                sqs.SendMessage2(40, SqsUSwest2Endpoint, 40, "DeadLetterQueue");
                sqs.SendMessage2(40, SqsUSwest2Endpoint, 40, "RW_LG");*/

                //sqs.RecevieMessage2(25, SqsUSeastEndpoint, Queuename);
                // sqs.ReceviveMessage(SqsUSeastEndpoint, Queuename);
                // stop.Stop();
                //Console.WriteLine(stop.Elapsed);
                

            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
            }
                
            Console.WriteLine("Press Enter to continue...");
            Console.Read();
        }


 
    }
}

