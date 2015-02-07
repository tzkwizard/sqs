using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net.ConnectionPool;
using Nest;

namespace ElasticSearch
{
    class Program
    {
        public static Uri Node;
        public static ConnectionSettings Settings;
        public static ElasticClient Client;

        static void Main(string[] args)
        {
            Node = new Uri("http://localhost:9200/");
            var connectionPool = new SniffingConnectionPool(new[] { Node });
            Settings =new ConnectionSettings(Node,defaultIndex:"azure");
            Client =new ElasticClient(Settings);

           // CreateIndex("azure");
          // InsertData();

            //Termquery();
            MathPhrase();
            Fillter();
            Console.ReadLine();
        }

        public static void CreateIndex(string name)
        {

            var indexsettings = new IndexSettings();
            indexsettings.NumberOfReplicas = 1;
            indexsettings.NumberOfShards = 5;
           
            Client.CreateIndex(c => c
                .Index(name)
                .InitializeUsing(indexsettings)
                .AddMapping<ActivityLog>(m => m.MapFromAttributes())); 
        }

        public static void InsertData()
        {
            var newPost = new ActivityLog
            {
                IpAddress = "21",
                Time = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Desciption = "azure elastic 1"
            };
            Client.Index(newPost);
            Console.WriteLine("inserted data");
            
        }

        public static void Termquery()
        {
            var result = Client.Search<ActivityLog>(s=>s
                         .Query(p=>p.Term(q=>q.Desciption,"azure")));
        }

        public static void MathPhrase()
        {
            var res = Client.Search<ActivityLog>(s=>s
                           .Query(q => q.MatchPhrase(m => m.OnField("ipAddress").Query("1"))));
        }

        public static void Fillter()
        {
            var res2 = Client.Search<ActivityLog>(s => s
                .Query(q => q.Term(p => p.Desciption, "azure"))
                .Filter(f=>f.Range(r=>r.OnField("ipAddress").Greater("0"))));
        }

    }
}
