using BluetoothMesh.Core.Domain;

namespace BluetoothMesh.UI.MVVM.Models
{
    public class NodeModel
    {
        public NodeModel(Node node)
        {
            NodeId = node.Id;
            Address = node.Address;
        }

        public int NodeId { get; set; }
        public Address Address { get; set; }
    }
}
