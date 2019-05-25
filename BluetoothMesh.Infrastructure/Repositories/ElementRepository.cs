using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BluetoothMesh.Infrastructure.Repositories
{
    public class ElementRepository : IElementRepository
    {
        private readonly IBluetoothMeshContext _context;

        public ElementRepository(IBluetoothMeshContext context)
        {
            _context = context;
        }

        public Element GetByAddress(int id)
        {
            return _context.Elements.FirstOrDefault(x => x.Address.Value == id);
        }

        public IEnumerable<Element> GetAll()
        {
            return _context.Elements;
        }

        public IEnumerable<Element> GetByNodeId(int nodeId)
        {
            return _context.Elements.Where(x => x.NodeId == nodeId);
        }
    }
}
