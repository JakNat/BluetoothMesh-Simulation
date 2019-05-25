using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.UDP;
using NetworkCommsDotNet.DPSBase;
using System.Net;

namespace BluetoothMesh.Infrastructure.Services
{
    public class BroadcastService : IBroadcastService
    {
        private readonly INodeRepository _baseNodeRepository;

        public BroadcastService(INodeRepository baseNodeRepository)
        {
            _baseNodeRepository = baseNodeRepository;
        }

        public void SendBroadcast<T>(Node baseNode, T request)
        {
            var nodesInRange = _baseNodeRepository.GetAllInRange(baseNode);

            foreach (var node in nodesInRange)
            {
                var endPoint = new IPEndPoint(IPAddress.Broadcast, node.Id);
                // typeof(T).Name -> pobiera nazwe klasy
                UDPConnection.SendObject(typeof(T).Name, request, endPoint, new SendReceiveOptions<ProtobufSerializer>());
            }
        }
    }
}
