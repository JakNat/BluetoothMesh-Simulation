using ProtoBuf;
using System;

namespace BluetoothMesh.Core.Domain.Requests
{
    /// <summary>
    /// Wiadomość wysyłana między węzły
    /// ProtoContract służy do automatycznej serializacji i deserializacji obiektu do postaci bajtów 
    /// </summary>
    [ProtoContract]

    //dziedziczenie protobuf
    // https://nirmalyabhattacharyya.com/2013/09/10/protobuf-net-and-inheritance-hierarchies/
    //[ProtoInclude(100, typeof(GetRequest))]
    //[ProtoInclude(200, typeof(SetRequest))]
    //[ProtoInclude(300, typeof(StatusRequest))]
    public class BaseRequest
    {
        public BaseRequest()
        {
            RequestId = Guid.NewGuid();
        }

        [ProtoMember(1)]
        public Guid RequestId { get; set; }

        /// <summary> 3.4.4.7
        /// The DST field is a 16-bit value that identifies the element or elements that this Network PDU is directed towards.
        /// This address shall be a unicast address, a group address, or a virtual address.
        /// The DST field is set by the originating node and is untouched by the network layer in nodes operating as a Relay node.
        /// </summary>
        [ProtoMember(2)]
        public Address DST { get; set; }


        /// <summary> 3.4.4.6
        /// The SRC field is a 16-bit value that identifies the element that originated this Network PDU. This address shall be a unicast address.
        /// The SRC field is set by the originating element and untouched by nodes operating as a Relay node.
        /// </summary>
        [ProtoMember(3)]
        public Address SRC { get; set; }

        [ProtoMember(5)]
        public int Heartbeats { get; set; }

        [ProtoMember(4)]
        public MessageType MessageType { get; set; }

        [ProtoMember(6)]
        public int Parameters { get; set; }

        [ProtoMember(7)]
        public Address BroadCastingNodeAddress { get; set; }

        [ProtoMember(8)]
        public Procedure Procedure { get; set; }
    }

    public enum MessageType
    {
        GET,
        SET,
        STATUS
    }

    public enum Procedure
    {
        DefaultTTL,
        GATTProxy,
        Friend,
        Relay,
        SubscriptionList,
        Light
    }
}
