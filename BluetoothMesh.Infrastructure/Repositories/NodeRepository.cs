using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BluetoothMesh.Infrastructure.Repositories
{
    /// <summary>
    /// repo dla nodów 
    /// </summary>
    public class NodeRepository : INodeRepository
    {
        private readonly IBluetoothMeshContext context;

        public NodeRepository(IBluetoothMeshContext context)
        {
            this.context = context;
        }

        public void Add(Node baseNode)
        {
            context.Nodes.ToList().Add(baseNode);
        }

        public Node Get(int id)
        {
            return context.Nodes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Node> GetAll()
        {
            return context.Nodes;
        }

        public IEnumerable<Node> GetAllInRange(Node baseNode)
        {
            var pickedNodes = new List<Node>();
            foreach (var node in context.Nodes.Where(x => x.Id != baseNode.Id))
            {
                var distance = baseNode.Posistion.DistanceTo(node.Posistion);
                var probability = 1 / ((Math.Pow(distance,2)) / 200 + 1) * 100;
                if (new Random().Next(100) <= probability)
                {
                    pickedNodes.Add(node);
                }
            }

            return pickedNodes;
        }

        public IEnumerable<Node> GetAllSubscribed()
        {
            throw new NotImplementedException();
        }
    }
}
