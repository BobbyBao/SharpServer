using MasterServer;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using GrainInterfaces;
using Orleans;
using SharpServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace GateServer
{
    public class GateServerHandler : Connection
    {
        IClusterClient clusterClient;
        public GateServerHandler(IClusterClient clusterClient)
        {
            this.clusterClient = clusterClient;
        }

        protected override void ChannelRead0(IChannelHandlerContext context, IByteBuffer message)
        {
            int msgType = message.GetInt(4);
            int svrID = msgType / 10000;
            int msgLen = message.GetInt(0);
            var msgData = message.GetIoBuffer(8, msgLen - 8);

            Log.Info("msg : {0}", msgType);

            IGateMessage gate = null;
            if (svrID == 1)
            {
                gate = clusterClient.GetGrain<IGateMaster>(0);
            }

            if (svrID == 3)
            {
                gate = clusterClient.GetGrain<IGateBattle>(0);
            }

            if(gate != null)
            {
                Task.Run(async () =>
                {
                    var response = await gate.SendMessage(msgType, msgData.ToArray());
                    if (response != null)
                    {
                        //约定response msg id = req + 1
                        await Send(msgType + 1, response);
                    }
                });
            }

        }

    }

    public class GateServer : ServerApp
    {

        IClusterClient clusterClient;

        public GateServer(IClusterClient clusterClient) : base()
        {
            this.clusterClient = clusterClient;
        }

        protected override Task OnStart()
        {
            var t = base.OnStart();

            return t;
        }

        protected override Connection CreateConnection()
        {
            return new GateServerHandler(clusterClient);
        }

    }
}
