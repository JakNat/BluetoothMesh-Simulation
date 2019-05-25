using BluetoothMesh.Core.Domain.Models;
using System.Collections.Generic;

namespace BluetoothMesh.Core.Domain.Elements
{
    public class Element
    {
        public Element()
        {
            Models = new Dictionary<ModelType, Model>();
            Subscriptions = new List<Address>();
        }

        public int Id { get; set; }
        public Address Address { get; set; }
        public List <Address> Subscriptions{ get; set; }
        public Dictionary<ModelType, Model> Models { get; set; }
        public ushort NodeId { get; set; }

        public void AddModel(ModelType modelType, Model model)
        {
            model.ElementId = Address.Value;
            Models.Add(modelType, model);
        }
    }
}
