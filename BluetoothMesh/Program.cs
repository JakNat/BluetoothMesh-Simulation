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
                Heartbeats = 3,
                Message = "elo kto pl",
                TargetNodeId = 7
            };

            context.NodeServers[0].Send(baseRequest);

            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);

        }
    }
}
