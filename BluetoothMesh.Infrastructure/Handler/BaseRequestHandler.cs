using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Linq;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class BearerLayerHandlers : ICommandHandler<BaseRequestCommand>
    {
        private readonly IBroadcastService _broadcastService;
        private readonly IModelRepository<Model> _modelRepository;
        private readonly IElementRepository _elementRepository;

        public BearerLayerHandlers(IBroadcastService broadcastService, IModelRepository<Model> modelRepository,
            IElementRepository elementRepository)
        {
            _broadcastService = broadcastService;
            _modelRepository = modelRepository;
            _elementRepository = elementRepository;
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

            if (incomingObject.Heartbeats >= 2 
                && Node.ConfigurationServerModel.Relay 
                && Node.ConfigurationServerModel.CompositionData.IsRelay)
            {
                var newRequest = incomingObject;
                newRequest.Heartbeats--;
                newRequest.BroadCastingNodeAddress = Node.Address;
                //System.Threading.Thread.Sleep(2500);
                
                _broadcastService.SendBroadcast(Node, newRequest);
            }

            Model model;
            switch (incomingObject.DST.AddressType)
            {
                case AddressType.Unassigned:
                    break;
                case AddressType.Unicast:
                    var element = Node.Elements.Values.FirstOrDefault(x => x.Address.Equals(incomingObject.DST));
                    model = element?.Models.First(x => x.Value.Procedures.Contains(incomingObject.Procedure)).Value;
                    model?.Dispatch(incomingObject, nodeServer);
                    break;
                case AddressType.Virtual:
                    break;
                case AddressType.Group:
                    foreach (var elem in Node.Elements.Values.Where(a => a.Subscriptions.Exists(x => x.Equals(incomingObject.DST))))
                    {
                        model = elem?.Models.First(x => x.Value.Procedures.Contains(incomingObject.Procedure)).Value;
                        model?.Dispatch(incomingObject, nodeServer);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
