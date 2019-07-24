using DotNetty.Transport.Channels;
using ProtoModel;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterServer
{
    public class MasterServer : ServerApp
    {
        public MasterServer(string[] args) : base(args)
        {
        }

        MsgHandler handler;

        protected override void OnConnect(MsgHandler handler)
        {
            var p = new Person
            {
                Name = "Test Person"
            };

            handler.Send(101, p);
            this.handler = handler;
        }

        protected override void OnRun()
        {
            Task.Run(Listen);

            while (true)
            {
                Thread.Sleep(1000);

                if(handler != null)
                {
                    var p = new Person
                    {
                        Name = "Test Person"
                    };

                    handler.Send(101, p);

                }
            }
        }
    }
}
