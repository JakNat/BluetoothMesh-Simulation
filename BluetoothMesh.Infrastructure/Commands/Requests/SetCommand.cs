using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Configuration;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class SetCommand : ICommand
    {
        public SetCommand(NodeServer nodeServer, SetRequest setRequest)
        {
            NodeServer = nodeServer;
            SetRequest = setRequest;
        }

        public NodeServer NodeServer { get; set; }
        public SetRequest SetRequest { get; set; }
    }
}
