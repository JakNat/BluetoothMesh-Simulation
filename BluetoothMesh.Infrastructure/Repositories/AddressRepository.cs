using System.Collections.Generic;
using System.Linq;
using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Repositories;
using BluetoothMesh.Infrastructure.DBL;

namespace BluetoothMesh.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IBluetoothMeshContext _context;

        public AddressRepository(IBluetoothMeshContext context)
        {
            _context = context;
        }

       

        public IEnumerable<Address> GetAllGroup()
        {
            return _context.Elements.ToList().SelectMany(x => x.Subscriptions).Distinct();
        }

        public IEnumerable<Address> GetAllUnicast()
        {
            return _context.Elements.ToList().Select(x => x.Address).Distinct();
        }

        public IEnumerable<Address> GetAllVirtual()
        {
            return new List<Address>();
        }
        public IEnumerable<Address> GetAll()
        {

            return GetAllGroup().Concat(GetAllUnicast()).Concat(GetAllVirtual()).Distinct();
        }
    }
}
