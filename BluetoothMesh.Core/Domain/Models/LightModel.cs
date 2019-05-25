using System;
using System.Collections.Generic;
using BluetoothMesh.Core.Domain.Requests;

namespace BluetoothMesh.Core.Domain.Models
{
    public class LightModel : Model
    {
        public bool ON { get; set; }

        public LightModel()
        {
            Procedures = new List<Procedure>()
            {
                Procedure.Light
            };
            //SubscriptionList.Add(GroupAddressesProvider.Dictionary[GroupAddresses.AllLights]);
        }


        public override void Dispatch(BaseRequest message, INodeBearer nodeBearer)
        {

            var node = nodeBearer.Node;
            Console.WriteLine($"Got message LightModel - Nodeid = {node.Id}");
            var status = new BaseRequest()
            {
                DST = message.SRC,
                Procedure = message.Procedure,
                Heartbeats = node.ConfigurationServerModel.DefaultTTL,
                MessageType = MessageType.STATUS,
                SRC = new Address(AddressType.Unicast,(ushort)ElementId)
            };

            switch (message.Procedure)
            {
                case Procedure.Light:
                    if (message.MessageType == MessageType.SET)
                        ON = message.Parameters == 1;
                    status.Parameters = Convert.ToInt32(ON);
                    break;
                default:
                    break;
            }

            nodeBearer.Send(status);
        }
    }
}
