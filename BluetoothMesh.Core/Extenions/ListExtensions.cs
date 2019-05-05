using BluetoothMesh.Core.Domain;
using System.Collections.Generic;

namespace BluetoothMesh.Core.Extenions
{
    public static class ListExtensions
    {
        public static List<Address> AddIfNotExist(this List<Address> addresses, Address address)
        {
            if (addresses.Exists(x => x.Value == address.Value))
            {
                return addresses;
            }
            else
            {
                addresses.Add(address);
                return addresses;
            }
        }
    }
}
