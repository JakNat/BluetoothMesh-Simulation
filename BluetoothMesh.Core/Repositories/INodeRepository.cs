using BluetoothMesh.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Repositories
{
    public interface INodeRepository: IRepository
    {
        IEnumerable<Node> GetAll();
        IEnumerable<Node> GetAllInRange(Node baseNode);
        IEnumerable<Node> GetAllSubscribed();
        Node Get(int id);
        void Add(Node baseNode);
    }
}
