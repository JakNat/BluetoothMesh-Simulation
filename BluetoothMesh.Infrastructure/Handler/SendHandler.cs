using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class SendHandler : ICommandHandler<SendCommand>
    {
        private readonly IBroadcastService _broadcastService;

        public SendHandler(IBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }
        public void Handle(SendCommand command)
        {
            _broadcastService.SendBroadcast(command.Node,command.BaseRequest);
        }
    }
}
