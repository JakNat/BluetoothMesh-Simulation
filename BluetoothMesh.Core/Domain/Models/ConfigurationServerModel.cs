using BluetoothMesh.Core.Domain.Models.States;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Core.Extenions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Domain.Models
{

    /* 4.4.1
     * This model is used to represent a mesh network configuration of a device.
     * The model shall be supported by a primary element and shall not be supported by any secondary elements.
     * The application-layer security on the Configuration Server model shall use the device key established
     * during provisioning.
     */
    public class ConfigurationServerModel : Model
    {
        public ConfigurationServerModel(Features features)
        {
            CompositionData = new CompositionData(features);
            DefaultTTL = 5;
            Procedures = new List<Procedure>
            {
                Procedure.DefaultTTL,
                Procedure.Friend,
                Procedure.GATTProxy,
                Procedure.Relay,
                Procedure.SubscriptionList,
            };
            Friend = features.Friend;
            GATTProxy = features.Proxy;
            Relay = features.Relay;
        }

        public CompositionData CompositionData { get; set; }

        /// <summary>
        /// The Default TTL state determines the TTL value used when sending messages.
        /// The Default TTL is applied by the access layer unless the application specifies a TTL. 
        /// The Default TTL values are defined in Table 4.10.
        /// </summary>
        public int DefaultTTL { get; set; }

        /// <summary>
        /// The Relay state indicates support for the Relay feature.
        /// If the Relay feature is supported, then this also indicates and controls whether the Relay feature 
        /// is enabled or disabled. The values are defined in Table 4.11.
        /// </summary>
        private bool _relay;
        public bool Relay
        {
            get { return _relay; }
            set
            {
                if (value)
                {
                    //SubscriptionList.AddIfNotExist(GroupAddressesProvider.Dictionary[GroupAddresses.AllRelays]);
                }
                else
                {
                    //SubscriptionList.RemoveAll(x => x.GuidId == GroupAddressesProvider.Dictionary[GroupAddresses.AllRelays].GuidId);
                }
                _relay = value;
            }
        }

        /// <summary>
        /// The GATT Proxy state indicates if the Proxy feature (see Section 3.4.6.2) is supported.
        /// If the feature is supported, the state indicates and controls the Proxy feature.
        /// </summary>
        private bool _GATTProxy;
        public bool GATTProxy
        {
            get { return _GATTProxy; }
            set
            {
                if (value)
                {
                    //SubscriptionList.AddIfNotExist(GroupAddressesProvider.Dictionary[GroupAddresses.AllProxies]);
                }
                else
                {
                    //SubscriptionList.RemoveAll(x => x.GuidId == GroupAddressesProvider.Dictionary[GroupAddresses.AllProxies].GuidId);
                }
                _GATTProxy = value;
            }
        }

        /// <summary>
        /// The Friend state indicates support for the Friend feature. If Friend feature is supported,
        /// then this also indicates and controls whether Friend feature is enabled or disabled.
        /// The values for this state are defined in Table 4.16.
        /// </summary>
        private bool _friend;
        public bool Friend
        {
            get { return _friend; }
            set
            {
                if (value)
                {
                    //SubscriptionList.AddIfNotExist(GroupAddressesProvider.Dictionary[GroupAddresses.AllFriends]);
                }
                else
                {
                    //SubscriptionList.RemoveAll(x => x.GuidId == GroupAddressesProvider.Dictionary[GroupAddresses.AllFriends].GuidId);
                }
                _friend = value;
            }
        }

        public override void Dispatch(BaseRequest message, INodeBearer nodeBearer)
        {

            var node = nodeBearer.Node;
            Console.WriteLine($"Got message ConfigurationServerModel - Nodeid = {node.Id}");
            node.StatusFlag = 2;
            System.Threading.Thread.Sleep(100);
            node.StatusFlag = 0;
            var status = new BaseRequest()
            {
                DST = message.SRC,
                Procedure = message.Procedure,
                Heartbeats = node.ConfigurationServerModel.DefaultTTL,
                MessageType = MessageType.STATUS,
                SRC = new Address(AddressType.Unicast, (ushort)ElementId)
            };

            switch (message.Procedure)
            {
                case Procedure.DefaultTTL:


                    if (message.MessageType == MessageType.SET)

                        DefaultTTL = message.Parameters;

                    status.Parameters = DefaultTTL;
                    break;
                case Procedure.GATTProxy:
                    if (message.MessageType == MessageType.SET)
                        GATTProxy = message.Parameters == 1;

                    status.Parameters = Convert.ToInt32(GATTProxy);
                    break;
                case Procedure.Friend:
                    if (message.MessageType == MessageType.SET)
                        Friend = message.Parameters == 1;
                    status.Parameters = Convert.ToInt32(Friend);
                    break;
                case Procedure.Relay:
                    if (message.MessageType == MessageType.SET)
                        Relay = message.Parameters == 1;
                    status.Parameters = Convert.ToInt32(Relay);

                    break;
                case Procedure.SubscriptionList:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentException($"Procedure not found in ConfigurationServerModel");
            }


            nodeBearer.Send(status);
        }
    }
}
