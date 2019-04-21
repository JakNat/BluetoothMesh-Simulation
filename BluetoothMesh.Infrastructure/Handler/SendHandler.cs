using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class SendHandler<T> : ICommandHandler<SendCommand<T>> where T : BaseRequest
    {
        private readonly IBroadcastService _broadcastService;

        public SendHandler(IBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }

        public void Handle(SendCommand<T> command)
        {
            _broadcastService.SendBroadcast(command.Node,command.BaseRequest);
        }
    }
}
