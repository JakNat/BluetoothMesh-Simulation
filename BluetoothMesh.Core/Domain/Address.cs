using ProtoBuf;
using System;

namespace BluetoothMesh.Core.Domain
{
    public enum AddressType
    {
        Unassigned,
        Unicast,
        Virtual,
        Group
    }

    /* 3.4.2
     * The network layer defines four basic types of addresses: unassigned, unicast, virtual, and group.
     */
     [ProtoContract]
    public class Address
    {
        private Address()
        {

        }
        public Address(AddressType addressType, ushort value = 0)
        {
            AddressType = addressType;
            Value = value;
            if(addressType == AddressType.Virtual)
            {
                GuidId = Guid.NewGuid();
            }
        }
        [ProtoMember(1)]
        public ushort Value { get; set; }

        [ProtoMember(2)]
        public Guid GuidId { get; set; }

        [ProtoMember(3)]
        public AddressType AddressType { get; set; }

        public override string ToString()
        {
            if (AddressType == AddressType.Virtual)
            {
                return GuidId.ToString();
            }
            return Value.ToString();
        }
    }
}
