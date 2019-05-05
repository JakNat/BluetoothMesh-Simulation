using BluetoothMesh.Core.Domain;

namespace BluetoothMesh.Infrastructure.Services
{
    public interface IBroadcastService
    {
        void SendBroadcast<T>(Node baseNode, T request);
    }
}