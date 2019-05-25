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
            return context.Nodes
                .Where(x => x.Posistion.DistanceTo(baseNode.Posistion) <= baseNode.Range
                && x.Id != baseNode.Id);
        }

        public IEnumerable<Node> GetAllSubscribed()
        {
            throw new NotImplementedException();
        }
    }
}
