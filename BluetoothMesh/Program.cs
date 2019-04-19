using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Configuration;
using BluetoothMesh.Infrastructure.DBL;
using BluetoothMesh.Infrastructure.Repositories;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Collections.Generic;

namespace BluetoothMesh
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new BluetoothMeshContext();
            var nodeRepository = new BaseNodeRepository<BaseNode>(context);
            var broadcastService = new BroadcastService(nodeRepository);

            foreach (var nodeServer in context.NodeServers)
            {
                nodeServer.SetBroadCastService(broadcastService);
                nodeServer.RegisterBasicResponse();
            }

            // testowy request 
            BaseRequest baseRequest = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "elo kto pl",
                TargetNodeId = 7
            };

            BaseRequest multicastBaseRequestA = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "multi król",
                TargetNodeId = MulticastProvider.ALL_NODES.GroupId
            };

            BaseRequest multicastBaseRequestB = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "hehe",
                TargetNodeId = MulticastProvider.KITCHEN.GroupId
            };

            context.NodeServers[0].Send(baseRequest);
            context.NodeServers[0].Send(multicastBaseRequestA);
            context.NodeServers[0].Send(multicastBaseRequestB);

            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);

        }
    }
}
