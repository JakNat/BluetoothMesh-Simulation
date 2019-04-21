using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Configuration;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class StatusCommand : ICommand
    {
        public StatusCommand(NodeServer nodeServer, StatusRequest statusRequest)
        {
            NodeServer = nodeServer;
            StatusRequest = statusRequest;
        }

        public NodeServer NodeServer { get; set; }
        public StatusRequest StatusRequest { get; set; }
    }
}
