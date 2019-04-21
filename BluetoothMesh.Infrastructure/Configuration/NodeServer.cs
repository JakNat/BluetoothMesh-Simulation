using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;

namespace BluetoothMesh.Infrastructure.Configuration
{
    public class NodeServer
    {
        private ICommandDispatcher _commandDispatcher;

        public NodeServer(BaseNode node)
        {
            Node = node;
            RegisterBasicResponse();
        }

        public BaseNode Node { get; set; }
        public List<Guid> ReceivedRequests { get; set; } = new List<Guid>();

        public void SetDispacher(ICommandDispatcher broadcastService)
        {
            _commandDispatcher = broadcastService;
        }

        public void Send<T>(T request) where T : BaseRequest
        {
            request.BroadCastingNodeId = Node.Id;
            var command = new SendCommand(this.Node, request);
            _commandDispatcher.Dispatch(command);
        }

        #region Packer handler delegates
        public void BasicResponse(PacketHeader packetHeader, Connection connection, BaseRequest incomingObject)
        {
            var command = new BaseRequestCommand(this, incomingObject);
            _commandDispatcher.Dispatch(command);
        }
        #endregion

        #region Register incoming pack handlers
        public void RegisterBasicResponse()
        {
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<BaseRequest>("BasicMessage", BasicResponse);
        }
        #endregion
    }
}
