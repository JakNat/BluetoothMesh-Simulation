using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using CommonServiceLocator;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Infrastructure.Configuration
{
    public class BearerLayer
    {
        private readonly ICommandDispatcher _dispatcher;

        public BearerLayer(ICommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        public static void BasicResponse(PacketHeader packetHeader, Connection connection, BaseRequest incomingObject)
        {
            var command = new BaseRequestCommand(null, incomingObject);
            ICommandDispatcher commandDispatcher = (ICommandDispatcher)ServiceLocator.Current.GetInstance(typeof(ICommandDispatcher));
            commandDispatcher.Dispatch(command);
        }
    }
}
