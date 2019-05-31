using BluetoothMesh.Core.Domain;

namespace BluetoothMesh.Infrastructure.Handler
{
    public class NodeUpdate
    {
        public NodeUpdate()
        {
        }

        public ushort NodeId { get; set; }
        public Address From { get; internal set; }
    }
}