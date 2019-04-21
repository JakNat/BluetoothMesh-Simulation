using BluetoothMesh.Core.Domain.Requests;

namespace BluetoothMesh.Core.Requests
{
    public static class PacketType
    {
        public static readonly string BasicMessage = nameof(BaseRequest);

        public static readonly string GET = nameof(GetRequest);
        public static readonly string SET = nameof(SetRequest);
        public static readonly string STATUS = nameof(StatusRequest);
    }
}
