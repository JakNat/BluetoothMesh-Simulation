using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;

namespace BluetoothMesh.Core
{
    public interface INodeBearer
    {
        Node Node { get; set; }

        void Send<T>(T request) where T : BaseRequest;
    }
}