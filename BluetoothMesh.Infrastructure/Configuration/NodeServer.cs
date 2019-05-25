using BluetoothMesh.Core;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Requests;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using CommonServiceLocator;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;

namespace BluetoothMesh.Infrastructure.Configuration
{
    public class NodeBearer : INodeBearer
    {
        private ICommandDispatcher _commandDispatcher;

        public NodeBearer(Node node)
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
            ICommandDispatcher commandDispatcher = (ICommandDispatcher)ServiceLocator.Current.GetInstance(typeof(ICommandDispatcher));

            request.BroadCastingNodeAddress = Node.Address;
            var command = new SendCommand<T>(this.Node, request);
            commandDispatcher.Dispatch(command);
        }

        #region Packer handler delegates
        public void BasicResponse(PacketHeader packetHeader, Connection connection, BaseRequest incomingObject)
        {
            var command = new BaseRequestCommand(this, incomingObject);
            ICommandDispatcher commandDispatcher = (ICommandDispatcher)ServiceLocator.Current.GetInstance(typeof(ICommandDispatcher));
            commandDispatcher.Dispatch(command);
        }

        #endregion

        #region Register incoming pack handlers
        /// <summary>
        /// na ten moment zakładamy ze node jest w stanie nasłuchiwać każdy typ wiadomośći czyli baseRequest, get,set,status
        /// </summary>
        public void RegisterBasicResponse()
        {
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<BaseRequest>(PacketType.BasicMessage, BasicResponse);
        }
        #endregion
    }
}
