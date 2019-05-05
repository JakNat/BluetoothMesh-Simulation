using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Requests;
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

        public NodeServer(Node node)
        {
            Node = node;
            RegisterBasicResponse();
        }

        public Node Node { get; set; }
        public List<Guid> ReceivedRequests { get; set; } = new List<Guid>();

        public void SetDispacher(ICommandDispatcher broadcastService)
        {
            _commandDispatcher = broadcastService;
        }

        public void Send<T>(T request) where T : BaseRequest
        {
            request.BroadCastingNodeId = Node.Id;
            var command = new SendCommand<T>(this.Node, request);
            _commandDispatcher.Dispatch(command);
        }

        #region Packer handler delegates
        public void BasicResponse(PacketHeader packetHeader, Connection connection, BaseRequest incomingObject)
        {
            var command = new BaseRequestCommand(this, incomingObject);
            _commandDispatcher.Dispatch(command);
        }

        public void GetResponse(PacketHeader packetHeader, Connection connection, GetRequest incomingObject)
        {
            var command = new GetCommand(this, incomingObject);
            _commandDispatcher.Dispatch(command);
        }

        public void SetResponse(PacketHeader packetHeader, Connection connection, SetRequest incomingObject)
        {
            var command = new SetCommand(this, incomingObject);
            _commandDispatcher.Dispatch(command);
        }

        public void StatusResponse(PacketHeader packetHeader, Connection connection, StatusRequest incomingObject)
        {
            var command = new StatusCommand(this, incomingObject);
            _commandDispatcher.Dispatch(command);
        }
        #endregion

        #region Register incoming pack handlers
        /// <summary>
        /// na ten moment zakładamy ze node jest w stanie nasłuchiwać każdy typ wiadomośći czyli baseRequest, get,set,status
        /// </summary>
        public void RegisterBasicResponse()
        {
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<BaseRequest>(PacketType.BasicMessage, BasicResponse);
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<SetRequest>(PacketType.SET, SetResponse);
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<GetRequest>(PacketType.GET, GetResponse);
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<StatusRequest>(PacketType.STATUS, StatusResponse);
        }
        #endregion
    }
}
