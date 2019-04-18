using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Configuration;
using System.Collections.Generic;

namespace BluetoothMesh.Infrastructure.DBL
{
    /// <summary>
    /// bazowy kontekst aplikacji
    /// </summary>
    public class BluetoothMeshContext : IBluetoothMeshContext
    {
        public BluetoothMeshContext()
        {
            BaseNodes = new List<BaseNode>()
            {
                new BaseNode(1,new Posistion(1,1)),
                new BaseNode(2,new Posistion(2,10)),
                new BaseNode(3,new Posistion(8,6)),
                new BaseNode(4,new Posistion(16,6)),
                new BaseNode(5,new Posistion(14,10)),
                new BaseNode(6,new Posistion(16,18)),
                new BaseNode(7,new Posistion(24,16))
            };

            NodeServers = new List<NodeServer>();
            foreach (var node in BaseNodes)
            {
                NodeServers.Add(new NodeServer(node));
            }
        }

        public IEnumerable<BaseNode> BaseNodes { get; set; }

        public List<NodeServer> NodeServers { get; set; }
    }
}
