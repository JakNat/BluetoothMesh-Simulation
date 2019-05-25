using ProtoBuf;
using System;

namespace BluetoothMesh.Core.Domain
{   

    public enum AddressType
    {
        /// <summary>
        /// An unassigned address is an address in which the element of a node has not been configured yet 
        /// or no address has been allocated. 
        /// This may be used, for example, to disable message publishing of a model by setting the publish address
        /// of a model to the unassigned address.
        /// </summary>
        Unassigned,

        /// <summary>
        /// A unicast address is allocated to each element of a node for the lifetime of the node on the network
        /// by a Provisioner during provisioning as described in Section 5.4.2.5.
        /// The address may be unallocated by a Provisioner to allow the address to be reused using the procedure
        /// defined in Section 3.10.7.
        /// </summary>
        Unicast,

        /// <summary>
        /// A virtual address represents a set of destination addresses.
        /// Each virtual address logically represents a Label UUID,
        /// which is a 128-bit value that does not have to be managed centrally.
        /// One or more elements may be programmed to publish or subscribe to a Label UUID.
        /// The Label UUID is not transmitted and shall be used as the Additional Data field of
        /// the message integrity check value in the upper transport layer
        /// </summary>
        Virtual,

        /// <summary>
        /// A group address is an address that is programmed into zero or more elements.
        /// </summary>
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
            return AddressType == AddressType.Virtual ? 
                GuidId.ToString() :  Value.ToString();
        }

        public override bool Equals(object obj)
        {
            Address objAddress = (Address)obj;
            return AddressType == AddressType.Virtual ? 
                GuidId == objAddress.GuidId : Value == objAddress.Value;
        }
    }
}
