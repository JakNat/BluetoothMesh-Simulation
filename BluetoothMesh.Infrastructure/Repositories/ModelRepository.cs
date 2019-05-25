using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BluetoothMesh.Infrastructure.Repositories
{
    public class ModelRepository : IModelRepository<Model>
    {
        private readonly IBluetoothMeshContext _context;

        public ModelRepository(IBluetoothMeshContext context)
        {
            _context = context;
        }

        public Model Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model> GetAll()
        {
            return _context.Models;
        }

        public IEnumerable<Model> GetByElementId(int elementId)
        {
            return _context.Models.Where(x => x.ElementId == elementId);
        }
    }
}
