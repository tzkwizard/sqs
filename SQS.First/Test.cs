using System;
using System.Threading;

namespace SQS.First
{
     class MyState
    {
        public MyState(string info, int number)
        {
            this.Info = info; this.Number = number;
        }

        public string Info { get; set; }

        public int Number { get; set; }
    }

    public class Test
    {
        public void Testthread()
        {

             WaitCallback waitCallback = new WaitCallback ( MyThreadWork );           
            WaitCallback waitCallback2 = new WaitCallback(MyThreadWork2);
             ThreadPool.QueueUserWorkItem ( waitCallback, "1 thread" );        
              ThreadPool.QueueUserWorkItem ( waitCallback, "2 thread" );          
            MyState myState = new MyState("3 thread", 100);   // 增加自定义的线程参数类型         
            ThreadPool.QueueUserWorkItem(waitCallback2, myState);
            ThreadPool.QueueUserWorkItem(waitCallback2, new MyState("4 thread", 2));
            Console.WriteLine("MyState - Number : {0}", myState.Number);    // 读取线程改变后的 MyState         
            Console.ReadLine();
        }

        private  void MyThreadWork(object state)
        {
            Console.WriteLine("MyThreadWork start …… {0}", (string)state);
            Thread.Sleep(3000);
            Console.WriteLine("end…… {0}", (string)state);
        }

        private  void MyThreadWork2(object state)
        {
            Console.WriteLine("MyThreadWork2 start…… {0},{1}", ((MyState)state).Info, ((MyState)state).Number);
            Thread.Sleep(3000);
            ((MyState)state).Number += 1;      // 将 state的 Number 加 1         
            Console.WriteLine("end…… {0},{1}", ((MyState)state).Info, ((MyState)state).Number);
        }
    }
}