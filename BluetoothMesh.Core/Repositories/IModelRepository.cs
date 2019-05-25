using BluetoothMesh.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Repositories
{
    public interface IModelRepository<T> : IRepository where T : Model
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> GetByElementId(int elementId);
    }
}
