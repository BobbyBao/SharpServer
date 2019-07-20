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
            testMsg = Encoding.UTF8.GetBytes(new string('A', 100));
        }
    }
}
