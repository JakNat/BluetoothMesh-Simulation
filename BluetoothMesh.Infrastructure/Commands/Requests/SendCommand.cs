using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class SendCommand<T> : ICommand where T : BaseRequest
    {
        public SendCommand(BaseNode node, T baseRequest)
        {
            Node = node;
            BaseRequest = baseRequest;
        }

        public BaseNode Node { get; set; }
        public T BaseRequest { get; set; }
    }
}
