using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class SendCommand<T> : ICommand where T : BaseRequest
    {
        public SendCommand(Node node, T baseRequest)
        {
            Node = node;
            BaseRequest = baseRequest;
        }

        public Node Node { get; set; }
        public T BaseRequest { get; set; }
    }
}
