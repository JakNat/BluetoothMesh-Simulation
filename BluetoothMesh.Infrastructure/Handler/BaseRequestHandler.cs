using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;
using System;

namespace BluetoothMesh.Infrastructure.Handler
{
    class BaseRequestHandler : ICommandHandler<BaseRequestCommand>
    {
        private readonly IBroadcastService _broadcastService;

        public BaseRequestHandler(IBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }
        public void Handle(BaseRequestCommand command)
        {
            var incomingObject = command.IncomingObject;
            var nodeServer = command.NodeServer;
              if (nodeServer.ReceivedRequests.Contains(incomingObject.RequestId))
            {
                Console.WriteLine("Node nr " + nodeServer.Node.Id + " blocked from nr " + incomingObject.BroadCastingNodeId);
                return ;
            }

            nodeServer.ReceivedRequests.Add(incomingObject.RequestId);

            Console.WriteLine("Node nr " + nodeServer.Node.Id + " get message from {Node " + incomingObject.BroadCastingNodeId + "}");
            if (incomingObject.TargetNodeId == nodeServer.Node.Id)
            {
                Console.WriteLine("CONGRATS!!");
                return ;
            }

            if (incomingObject.Heartbeats == 0)
            {
                Console.WriteLine("Message died :<");
                return ;
            }

            var newRequest = incomingObject;
            newRequest.Heartbeats--;
            newRequest.BroadCastingNodeId = nodeServer.Node.Id;
            _broadcastService.SendBroadcast(nodeServer.Node, newRequest);
        }
    }
}
