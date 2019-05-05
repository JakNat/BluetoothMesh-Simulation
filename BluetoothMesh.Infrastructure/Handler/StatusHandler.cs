using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;
using System;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class StatusHandler : ICommandHandler<StatusCommand>
    {
        private readonly IBroadcastService _broadcastService;
        private readonly INodeRepository<Node> _baseNodeRepository;

        public StatusHandler(IBroadcastService broadcastService,
            INodeRepository<Node> baseNodeRepository)
        {
            _broadcastService = broadcastService;
            _baseNodeRepository = baseNodeRepository;
        }

        public void Handle(StatusCommand command)
        {
            //to do
            throw new NotImplementedException();
        }
    }
}
