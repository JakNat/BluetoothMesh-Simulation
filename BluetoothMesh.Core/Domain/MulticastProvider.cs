using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Domain
{
    public static class MulticastProvider
    {
        public static Multicast ALL_NODES = new Multicast(1111, "ALL_NODES");
        public static Multicast KITCHEN = new Multicast(1112, "KITCHEN");
        
        //...
    }
}
