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

        [ProtoMember(2)]
        public Address DST { get; set; }

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
