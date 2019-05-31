using System.Collections.Generic;
using BluetoothMesh.Core.Domain.Requests;
using System.Timers;
using BluetoothMesh.Core.Domain.Elements;
using BluetoothMesh.Core.Extenions;
using BluetoothMesh.Core.Domain.Models;
using System.ComponentModel;

namespace BluetoothMesh.Core.Domain
{

    public class Node : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        private int statusFlag;
        public int StatusFlag
        {
            get { return this.statusFlag; }
            set
            {
                this.statusFlag = value;
                if (value == 1 || value == 2)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(this.Id.ToString() + value.ToString()));
                }
            }
        }
        

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
            StatusFlag = 0;
        }
    }
}
