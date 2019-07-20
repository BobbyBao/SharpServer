namespace Test.Server
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using ServiceStack;
    using ServiceStack.Text;
    using SharpServer;

    /// <summary>
    /// 服务端处理事件函数
    /// </summary>
    public class PerfTestServerHandler : SharpServer.NetworkServer.MessageHandler
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Interlocked.Increment(ref Stats.recv);
            context.WriteAsync(message);
            Interlocked.Increment(ref Stats.send);
        }

    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddTime { get; set; }
        public  int Age { get; set; }
        public bool Status { get; set; }
    }

    public class RerfTestServer : ServerApp<PerfTestServerHandler>
    {
        void TestSerialize()
        {
            int count = 100000;
            var sw = new Stopwatch();
            sw.Start();


            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,                                   //格式化json字符串
                AllowTrailingCommas = true,                             //可以结尾有逗号
                                                                        //IgnoreNullValues = true,                              //可以有空值,转换json去除空值属性
                IgnoreReadOnlyProperties = true,                        //忽略只读属性
                PropertyNameCaseInsensitive = true,                     //忽略大小写
                                                                        //PropertyNamingPolicy = JsonNamingPolicy.CamelCase     //命名方式是默认还是CamelCase
            };


            User user = new User() { Id = 1, Name = "tyh", AddTime = DateTime.Now, Age = 15, Status = true };
            string temp = String.Empty;

            User user1 = new User();
            string temp1 = System.Text.Json.Serialization.JsonSerializer.ToString(user, options); //"{\"Id\": 2,\"Name\": \"xxc\",\"AddTime\": \"2016-09-07 10:10:10\",\"Age\": \"15\",\"Status\": \"true\"}";

            Console.WriteLine(temp1);
            temp = user.ToJsv();// TypeSerializer.SerializeToString(user);

            File.WriteAllText("test.json", temp);

            //Console.WriteLine(temp);



            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                string t = ServiceStack.Text.JsonSerializer.SerializeToString(user);
            }

            Console.WriteLine("ServiceStack-Ser:" + sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = ServiceStack.Text.JsonSerializer.DeserializeFromString<User>(temp1);
            }

            Console.WriteLine("ServiceStack-Des:" + sw.ElapsedMilliseconds);

            //对象转为json 字符串
            sw.Restart();
            for(int i = 0; i < count; i++)
            {
                string t = System.Text.Json.Serialization.JsonSerializer.ToString(user, options);
            }

            Console.WriteLine("System.Text.Json-Ser:" + sw.ElapsedMilliseconds);

            //json字符串转为集合
            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = System.Text.Json.Serialization.JsonSerializer.Parse<User>(temp1);
            }

            Console.WriteLine("System.Text.Json-Des:" + sw.ElapsedMilliseconds);


            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = Utf8Json.JsonSerializer.Serialize(user);
            }

            Console.WriteLine("Utf8Json-Ser:" + sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                var t = Utf8Json.JsonSerializer.Deserialize<User>(temp1);
            }

            Console.WriteLine("Utf8Json-Des:" + sw.ElapsedMilliseconds);
        }

        protected override void OnRun()
        {
            Task.Run(() => Server.Start<PerfTestServerHandler>(Port));

            TestSerialize();

            var sw = new Stopwatch();
            sw.Start();

            int lastRecv = 0;
            int lastSend = 0;
            int count = 0;
            while (true)
            {
                if(count < 1000000)
                {
                    Task.Run(async () =>
                    {
                        IByteBuffer initialMessage = Unpooled.Buffer(128);
                        initialMessage.WriteBytes(Stats.testMsg);
                        count += await Server.Broadcast(initialMessage);
                    });
                }

                if (sw.ElapsedMilliseconds >= 1000)
                {
                    sw.Restart();

                    Console.WriteLine("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
                    lastRecv = Stats.recv;
                    lastSend = Stats.send;
                }

                Thread.Sleep(1);
            }
        }
    }
}
