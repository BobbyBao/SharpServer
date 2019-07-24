using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmark
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddTime { get; set; }
        public int Age { get; set; }
        public bool Status { get; set; }
    }

    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class TestJson
    {
        int count = 1000;

        User user = new User() { Id = 1, Name = "tyh", AddTime = DateTime.Now, Age = 15, Status = true };
        string json;

        public TestJson()
        {

#if NETCOREAPP3_0
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,                                   //格式化json字符串
                AllowTrailingCommas = true,                             //可以结尾有逗号
                                                                        //IgnoreNullValues = true,                              //可以有空值,转换json去除空值属性
                IgnoreReadOnlyProperties = true,                        //忽略只读属性
                PropertyNameCaseInsensitive = true,                     //忽略大小写
                                                                        //PropertyNamingPolicy = JsonNamingPolicy.CamelCase     //命名方式是默认还是CamelCase
            };
#endif

            json = Utf8Json.JsonSerializer.PrettyPrint(Utf8Json.JsonSerializer.Serialize(user));

        }

        [BenchmarkCategory("Serialize"), Benchmark(Baseline = true)]
        public void ServiceStackSer()
        {
            for (int i = 0; i < count; i++)
            {
                string t = ServiceStack.Text.JsonSerializer.SerializeToString(user);
            }

        }

        [BenchmarkCategory("Serialize"), Benchmark]
        public void Utf8JsonSer()
        {
            for (int i = 0; i < count; i++)
            {
                var t = Utf8Json.JsonSerializer.Serialize(user);
            }
        }

#if NETCOREAPP3_0
        [BenchmarkCategory("Serialize"), Benchmark]
        public void SystemJsonSer()
        {
            for (int i = 0; i < count; i++)
            {
                var t = System.Text.Json.Serialization.JsonSerializer.ToString(user, options);
            }
        }
#endif

        [BenchmarkCategory("Deserialize"), Benchmark(Baseline = true)]
        public void ServiceStackDes()
        {
            for (int i = 0; i < count; i++)
            {
                var t = ServiceStack.Text.JsonSerializer.DeserializeFromString<User>(json);
            }

        }


        [BenchmarkCategory("Deserialize"), Benchmark]
        public void Utf8JsonDes()
        {
            for (int i = 0; i < count; i++)
            {
                var t = Utf8Json.JsonSerializer.Deserialize<User>(json);
            }
        }

#if NETCOREAPP3_0
        [BenchmarkCategory("Deserialize"), Benchmark]
        public void SystemJsonDes()
        {
            for (int i = 0; i < count; i++)
            {
                var t = System.Text.Json.Serialization.JsonSerializer.Parse<User>(json);
            }
        }
#endif

    }

}
