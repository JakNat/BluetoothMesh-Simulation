using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Repositories;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.UDP;
using NetworkCommsDotNet.DPSBase;
using System.Net;

namespace BluetoothMesh.Infrastructure.Services
{
    public class BroadcastService : IBroadcastService
    {
        private readonly BaseNodeRepository<BaseNode> _baseNodeRepository;

        public BroadcastService(BaseNodeRepository<BaseNode> baseNodeRepository)
        {
            _baseNodeRepository = baseNodeRepository;
        }

        public void SendBroadcast<T>(BaseNode baseNode, T request)
        {
            var nodesInRange = _baseNodeRepository.GetAllInRange(baseNode);

            foreach (var node in nodesInRange)
            {
                var endPoint = new IPEndPoint(IPAddress.Broadcast, node.Id);
                UDPConnection.SendObject("BasicMessage", request, endPoint, new SendReceiveOptions<ProtobufSerializer>());
            }
        }
    }
}
