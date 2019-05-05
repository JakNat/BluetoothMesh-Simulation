using BluetoothMesh.Core.Domain.Models.States;
using BluetoothMesh.Core.Domain.Requests;
using System;
using System.Collections.Generic;

namespace BluetoothMesh.Core.Domain.Models
{
    public abstract class Model
    {
        public Model()
        {
            SubscriptionList = new List<Address>();
            Address = new Address(AddressType.Virtual);
        }
        public Address Address { get; set; }
        public virtual List<Address> SubscriptionList { get; set; }
        public ModelPublication ModelPublication{ get; set; }

        public abstract void Dispatch(BaseRequest message, INodeBearer nodeBearer);

        public List<Procedure> Procedures { get; set; }
      
    }
}
