using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class Stats
    {
        public static int send = 0;
        public static int recv = 0;
        public static int sendBytes = 0;
        public static int recvBytes = 0;
        public static byte[] testMsg;

        static Stats()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.Append('A');
            }
            testMsg = Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
