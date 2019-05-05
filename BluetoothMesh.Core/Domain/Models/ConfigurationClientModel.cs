using BluetoothMesh.Core.Domain.Models;
using BluetoothMesh.Core.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Domain.Nodes.Elements.Models
{
    public class ConfigurationClientModel : Model
    {
        public ConfigurationClientModel()
        {
            Procedures = new List<Procedure>()
            {
                Procedure.DefaultTTL,
                Procedure.Friend,
                Procedure.GATTProxy,
                Procedure.Light,
                Procedure.Relay,
                Procedure.SubscriptionList
            };
        }
        public override void Dispatch(BaseRequest message, INodeBearer nodeBearer)
        {
            if (message.MessageType == MessageType.STATUS)
            {

                switch (message.Procedure)
                {
                    case Procedure.DefaultTTL:
                        Console.WriteLine($"{message.Procedure.ToString()} Value is {message.Parameters} -- SRC = {message.SRC}");
                        break;
                    case Procedure.GATTProxy:
                        Console.WriteLine($"{message.Procedure.ToString()} Value is {message.Parameters} -- SRC = {message.SRC}");
                        break;
                    case Procedure.Friend:
                        Console.WriteLine($"{message.Procedure.ToString()} Value is {message.Parameters} -- SRC = {message.SRC}");
                        break;
                    case Procedure.Relay:
                        Console.WriteLine($"{message.Procedure.ToString()} Value is {message.Parameters} -- SRC = {message.SRC}");
                        break;
                    case Procedure.SubscriptionList:
                        //DefaultTTL = message.Parameters == 1;
                        break;
                    case Procedure.Light:
                        Console.WriteLine($"{message.Procedure.ToString()} Value is {message.Parameters} -- SRC = {message.SRC}");
                        break;

                    default:
                        throw new ArgumentException($"Procedure not found in ConfigurationServerModel");
                }
            }

        }

        public void SendMessage(INodeBearer nodeBearer, BaseRequest message)
        {
            message.SRC = this.Address;
            message.Heartbeats = nodeBearer.Node.ConfigurationServerModel.DefaultTTL;
            nodeBearer.Send(message);
        }
    }
}
