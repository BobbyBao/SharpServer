using ServiceStack;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;

namespace Serialize
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddTime { get; set; }
        public int Age { get; set; }
        public bool Status { get; set; }
    }

    class Program
    {
        static void TestSerialize()
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
            for (int i = 0; i < count; i++)
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

        static void Main(string[] args)
        {
            TestSerialize();

            Console.ReadKey();
        }
    }
}
