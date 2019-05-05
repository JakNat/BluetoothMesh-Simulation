using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Linq;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class BaseRequestHandler : ICommandHandler<BaseRequestCommand>
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

            if (ReceivedRequests.Contains(incomingObject.RequestId))
            {
                return;
            }
            else
            {
                ReceivedRequests.Add(incomingObject.RequestId);
            }

            //Console.WriteLine("Node nr " + Node.Id + " get message from {Node " + incomingObject.BroadCastingNodeAddress.Value + "}");

            if (incomingObject.Heartbeats >= 2 && Node.ConfigurationServerModel.Relay)
            {
                var newRequest = incomingObject;
                newRequest.Heartbeats--;
                newRequest.BroadCastingNodeAddress = Node.Address;
                //System.Threading.Thread.Sleep(2500);
                _broadcastService.SendBroadcast(Node, newRequest);
            }
            switch (incomingObject.DST.AddressType)
            {
                case AddressType.Unassigned:
                    break;
                case AddressType.Unicast:
                    var element = Node.Elements.Values.FirstOrDefault(x => x.Address.Value == incomingObject.DST.Value);
                    element?.Models.Where(x => x.Value.Procedures.Contains(incomingObject.Procedure))
                        .ToList().ForEach(x => x.Value.Dispatch(incomingObject, nodeServer));
                    break;
                case AddressType.Virtual:
                    var model = Node.Elements.SelectMany(el => el.Value.Models.Values)
                   .FirstOrDefault(m => m.Address.GuidId == incomingObject.DST.GuidId &&
                   m.Procedures.Contains(incomingObject.Procedure));

                    model?.Dispatch(incomingObject, nodeServer);
                    break;
                case AddressType.Group:
                    var allModessls = Node.Elements.SelectMany(el => el.Value.Models);
                    //var a = allModessls.Where(x => x.SubscriptionList.Exists( y => y.Value == incomingObject.DST.Value));
                    var allModels = Node.Elements.SelectMany(el => el.Value.Models.Values)
                        .Where(m => m.SubscriptionList.Exists(a => a.Value == incomingObject.DST.Value) &&
                        m.Procedures.Contains(incomingObject.Procedure)).ToList();
                    allModels.ForEach(m => m.Dispatch(incomingObject, nodeServer));
                    break;
                default:
                    break;
            }
        }
    }
}
