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

            if (svrID == 1)
            {
                var res = new UserLoginRes
                {
                    Res = 0,
                    UserId = "Test user id."
                };

                var player = clusterClient.GetGrain<IPlayerGrain>(0);

                Task.Run(async ()=>
                {
                    var response = await player.SendMessage(msgType, msgData.ToArray());
                    if(response != null)
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

        protected override Connection CreateConnection()
        {
            return new GateServerHandler(clusterClient);
        }

    }
}
