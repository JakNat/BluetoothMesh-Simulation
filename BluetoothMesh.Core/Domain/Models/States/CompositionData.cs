namespace BluetoothMesh.Core.Domain.Models.States
{
    /* 4.2.1
     * The Composition Data state contains information about a node, the elements it includes,
     * and the supported models.
     * The Composition Data is composed of a number of pages of information.
     */
    public class CompositionData
    {
        public CompositionData(Features features)
        {
            IsFriend = features.Friend;
            IsProxy = features.Proxy;
            IsRelay = features.Relay;
        }

        /// <summary>
        /// 4.3
        /// </summary>
        public bool IsFriend { get; set; }
        public bool IsProxy { get; set; }
        public bool IsRelay { get; set; }
    }
}
