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
        }

        public int ModelId { get; set; }
        public int ElementId { get; set; }

        public ModelPublication ModelPublication{ get; set; }

        public List<Procedure> Procedures { get; set; }

        /// <summary>
        /// 3.7.4.2 Receiving an access message
        /// A message is delivered to the model for processing if all the following conditions are met:
        /// - The opcode (procedure) indicates the message is used by the model on this element.
        /// - The destination address is set to one of the following:
        /// --- the model’s element unicast address;
        /// --- a group or virtual address which the model’s element is subscribed to;
        /// --- a fixed group address of the primary element of the node as defined in Section 3.4.2.4.
        /// </summary>
        public abstract void Dispatch(BaseRequest message, INodeBearer nodeBearer);
      
    }
}
