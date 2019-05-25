using System.Collections.Generic;
using BluetoothMesh.Core.Domain.Requests;
using System.Timers;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Extenions;
using BluetoothMesh.Core.Domain.Models;

namespace BluetoothMesh.Core.Domain
{

    public class Node
    {
        /// <summary>
        /// traktujemy node id jako jego numer portu
        /// zakres 0 - 65535
        /// </summary>
        public ushort Id { get; protected set; }
        public Address Address { get; set; }
        public ConfigurationServerModel ConfigurationServerModel
        {
            get
            {
                return (ConfigurationServerModel)Elements[ElementType.primary].Models[ModelType.ConfigurationServer];
            }
        }

        public Dictionary<ElementType, Element> Elements { get; set; }
        public Listener Listener { get; set; }
        public double Range { get; set; } = 10;
        public Posistion Posistion { get; set; }

        public Node(ushort nodeId, Posistion posistion, Features features, Dictionary<ModelType,Model> primaryElementExtensionModels = null)
        {
            Id = nodeId;
            Address = new Address(AddressType.Unicast, nodeId);
            Posistion = posistion;
            Listener = new Listener(nodeId);

            Elements = new Dictionary<ElementType, Element>()
            {
                { ElementType.primary, new Element(){ Address = this.Address} }
            };

            var serverModel = new ConfigurationServerModel(features) { ElementId = Elements[ElementType.primary].Address.Value};
            Elements[ElementType.primary].AddModel(ModelType.ConfigurationServer, serverModel);

            if (primaryElementExtensionModels != null)
            {
                foreach (var item in primaryElementExtensionModels)
                {
                    Elements[ElementType.primary].AddModel(item.Key, item.Value);
                }
            }
            //if (primaryElementExtensionModels != null)
            //    Elements[ElementType.primary].Models.AddRange(primaryElementExtensionModels);
        }
    }
}
