using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BluetoothMesh.Core.Domain.Requests
{
    [ProtoContract]
    public class MulticastBaseRequest : BaseRequest
    {
        public int NumberOfNodesToDeliver { get; set; }
    }
}
