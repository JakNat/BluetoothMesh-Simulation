using BluetoothMesh.Core.Domain;
using System.Collections.Generic;

namespace BluetoothMesh.Core.Repositories
{
    public interface IAddressRepository : IRepository
    {
        IEnumerable<Address> GetAll();
        IEnumerable<Address> GetAllGroup();
        IEnumerable<Address> GetAllUnicast();
        IEnumerable<Address> GetAllVirtual();
    }
}
