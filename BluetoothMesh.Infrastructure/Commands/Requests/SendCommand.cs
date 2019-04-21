using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;

namespace BluetoothMesh.Infrastructure.Commands.Requests
{
    public class SendCommand : ICommand
    {
        public SendCommand(BaseNode node, BaseRequest baseRequest)
        {
            Node = node;
            BaseRequest = baseRequest;
        }

        public BaseNode Node { get; set; }
        public BaseRequest BaseRequest { get; set; }
    }
}
