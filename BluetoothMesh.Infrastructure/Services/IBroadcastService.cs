using BluetoothMesh.Core.Domain;

namespace BluetoothMesh.Infrastructure.Services
{
    public interface IBroadcastService
    {
        void SendBroadcast<T>(BaseNode baseNode, T request);
    }
}