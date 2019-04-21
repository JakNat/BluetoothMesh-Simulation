using ProtoBuf;
using System;

namespace BluetoothMesh.Core.Domain.Requests
{
    /// <summary>
    /// Wiadomość wysyłana między węzły
    /// ProtoContract służy do automatycznej serializacji i deserializacji obiektu do postaci bajtów 
    /// </summary>
    [ProtoContract]
    public class BaseRequest
    {
        public BaseRequest()
        {
            RequestId = Guid.NewGuid();
        }

        [ProtoMember(1)]
        public Guid RequestId { get; set; }

        [ProtoMember(2)]
        public int TargetNodeId { get; set; }

        [ProtoMember(3)]
        public int BroadCastingNodeId { get; set; }

        [ProtoMember(4)]
        public string Message { get; set; }

        [ProtoMember(5)]
        public int Heartbeats { get; set; }
    }
}
