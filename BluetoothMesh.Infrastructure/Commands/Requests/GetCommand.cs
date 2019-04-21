using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Configuration;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class GetCommand : ICommand
    {
        public GetCommand(NodeServer nodeServer, GetRequest getRequest)
        {
            NodeServer = nodeServer;
            GetRequest = getRequest;
        }

        public NodeServer NodeServer{ get; set; }
        public GetRequest GetRequest{ get; set; }
    }
}
