using BluetoothMesh.Core.Domain.Models;
using System.Collections.Generic;

namespace BluetoothMesh.Core.Domain.Elements
{

    public class Element
    {
        public Element()
        {
            Models = new Dictionary<ModelType, Model>();
        }

        public Address Address { get; set; }
        public Dictionary<ModelType, Model> Models { get; set; }

    }
}
