using BluetoothMesh.Core.Domain;
using BluetoothMesh.Infrastructure.Configuration;
using BluetoothMesh.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Domain.Nodes;
using BluetoothMesh.Core.Domain.Models;
using System;
using BluetoothMesh.Core.Domain.Nodes.Elements.Models;

namespace BluetoothMesh.Infrastructure.DBL
{
    public class BluetoothMeshContext : IBluetoothMeshContext
    {
        public int NodeCount { get; set; } = 0;
        public BluetoothMeshContext()
        {

            Nodes = new List<Node>()
            {
                new Node(1,new Posistion(1,1), new Features(){ Relay = false, Proxy = true, Friend = false}, GetClient()),
                new Node(2,new Posistion(2,10), new Features(){ Relay = true, Proxy = false, Friend = false}, GetLightBulb()),
                new Node(3,new Posistion(8,6), new Features(){ Relay = true, Proxy = false, Friend = true}),
                new Node(4,new Posistion(3,13), new Features(){ Relay = true, Proxy = false, Friend = false }),
                new Node(5,new Posistion(14,10), new Features(){ Relay = true, Proxy = false, Friend = false}, GetLightBulb()),
                new Node(6,new Posistion(16,18), new Features(){ Relay = true, Proxy = false, Friend = true}),
                new Node(7,new Posistion(24,16), new Features(){ Relay = true, Proxy = false, Friend = false }, GetLightBulb())
            };

            NodeServers = new List<NodeBearer>();
            foreach (var node in Nodes)
            {
                var nodeServer = new NodeBearer(node);
                NodeServers.Add(nodeServer);
            }
        }

        private static Dictionary<ModelType, Model> GetClient()
        {
            return new Dictionary<ModelType, Model>()
            {
                {ModelType.ConfigurationClient, new ConfigurationClientModel(){Address = new Address(AddressType.Virtual) } }
            };
        }

        private static Dictionary<ModelType, Model> GetLightBulb()
        {
            return new Dictionary<ModelType, Model>()
            {
                {ModelType.Light, new LightModel(){Address = new Address(AddressType.Virtual) } }
            };
        }

        public Node GetNodeById (int id)
        {
            Node searchedNode = (Node) Nodes.Where(node => node.Id == id).FirstOrDefault();
            return searchedNode;
        }

        public List<Node> Nodes { get; set; }

        public List<NodeBearer> NodeServers { get; set; }

        public List<Node> GetAllInRange(Node baseNode)
        {
            return (List<Node>)Nodes
                .Where(x => x.Posistion.DistanceTo(baseNode.Posistion) <= baseNode.Range
                && x.Id != baseNode.Id);
        }
    }
}
