using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Configuration;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class BaseRequestCommand : ICommand
    {
        public BaseRequestCommand(NodeServer nodeServer, BaseRequest incomingObject)
        {
            NodeServer = nodeServer;
            IncomingObject = incomingObject;
        }

        public NodeServer NodeServer{ get; set; }
        public BaseRequest IncomingObject{ get; set; }
        
    }
}
