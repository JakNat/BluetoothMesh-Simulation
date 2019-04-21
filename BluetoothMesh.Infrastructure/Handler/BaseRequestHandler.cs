using BluetoothMesh.Core.Domain;
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
            var Node = nodeServer.Node;
            var ReceivedRequests = nodeServer.ReceivedRequests;
            //to do
            //if (!Node.SubsribedNodes.Contains(incomingObject.BroadCastingNodeId))
            //{
            //    return;
            //}

            if (ReceivedRequests.Contains(incomingObject.RequestId))
            {
                //    Console.WriteLine("Node nr " + Node.Id + " blocked from nr " + incomingObject.BroadCastingNodeId);
                return;
            }

            if (Node.Function == Function.low_energy) { return; }

            ReceivedRequests.Add(incomingObject.RequestId);

            Console.WriteLine("Node nr " + Node.Id + " get message from {Node " + incomingObject.BroadCastingNodeId + "}");
            if (incomingObject.TargetNodeId == Node.Id)
            {
                Console.WriteLine("CONGRATS!!");
                return;
            }
            foreach (Multicast Group in Node.Subscribed)
            {
                if (incomingObject.TargetNodeId == Group.GroupId)
                {
                    Console.WriteLine("RECEIVED GROUP CAST ON " + Group.GroupName);
                }
            }

            if (incomingObject.Heartbeats == 0)
            {
                Console.WriteLine("Message died :<");
                return;
            }

            if (Node.Function == Function.friend)
            {
                if (Node.FriendNodes.Contains(incomingObject.TargetNodeId))
                {
                    Node.MessagesForLowPowerNodes.Add(incomingObject);
                    Console.WriteLine("GOT MESSAGE FOR LE NODE: " + incomingObject.TargetNodeId + ": " + incomingObject.Message);
                    return;
                }
            }

            var newRequest = incomingObject;
            newRequest.Heartbeats--;
            newRequest.BroadCastingNodeId = Node.Id;
            //System.Threading.Thread.Sleep(2500);
            _broadcastService.SendBroadcast(Node, newRequest);
        }
    }
}
