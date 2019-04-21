using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Configuration;
using BluetoothMesh.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace BluetoothMesh.Infrastructure.DBL
{
    public class BluetoothMeshContext : IBluetoothMeshContext
    {
        public int NodeCount { get; set; } = 0;
        public BluetoothMeshContext()
        {
            BaseNodes = new List<BaseNode>()
            {
                new BaseNode(1,new Posistion(1,1), Function.relay),
                new BaseNode(2,new Posistion(2,10), Function.friend),
                new BaseNode(3,new Posistion(8,6), Function.relay, MulticastProvider.ALL_NODES, MulticastProvider.KITCHEN),
                new BaseNode(4,new Posistion(3,13), Function.low_energy),
                new BaseNode(5,new Posistion(14,10), Function.relay, MulticastProvider.ALL_NODES),
                new BaseNode(6,new Posistion(16,18), Function.relay, MulticastProvider.KITCHEN),
                new BaseNode(7,new Posistion(24,16), Function.relay)
            };

            NodeCount = 7;
            GetNodeById(4).SetParentNode(GetNodeById(2));

            NodeServers = new List<NodeServer>();
            foreach (var node in BaseNodes)
            { 
                var nodeServer = new NodeServer(node);
                NodeServers.Add(nodeServer);
            }
        }

        public BaseNode GetNodeById (int id)
        {
            BaseNode searchedNode = (BaseNode) BaseNodes.Where(node => node.Id == id).FirstOrDefault();
            return searchedNode;
        }

        public List<BaseNode> BaseNodes { get; set; }

        public List<NodeServer> NodeServers { get; set; }
    }
}
