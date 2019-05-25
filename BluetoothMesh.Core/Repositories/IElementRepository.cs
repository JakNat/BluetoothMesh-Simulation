using BluetoothMesh.Core.Domain.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Repositories
{
    public interface IElementRepository : IRepository
    {
        IEnumerable<Element> GetAll();
        Element GetByAddress(int id);
        IEnumerable<Element> GetByNodeId(int elementId);
    }
}
