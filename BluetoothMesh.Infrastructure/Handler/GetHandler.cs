using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.Commands;
using BluetoothMesh.Infrastructure.Commands.Requests;
using BluetoothMesh.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class GetHandler : ICommandHandler<GetCommand>
    {
        private readonly IBaseNodeRepository<BaseNode> _baseNodeRepository;
        private readonly IBroadcastService _broadcastService;

        public GetHandler(IBroadcastService broadcastService,
            IBaseNodeRepository<BaseNode> baseNodeRepository)
        {
            _baseNodeRepository = baseNodeRepository;
            _broadcastService = broadcastService;
        }

        public void Handle(GetCommand command)
        {
            //to do
            throw new NotImplementedException();
        }
    }
}
