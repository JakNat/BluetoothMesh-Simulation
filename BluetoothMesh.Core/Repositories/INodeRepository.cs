using BluetoothMesh.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Repositories
{
    public interface IBaseNodeRepository<T> : IRepository where T : Node
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllInRange(T baseNode);
        IEnumerable<T> GetAllSubscribed();
        T Get(int id);
        void Add(T baseNode);
    }
}
