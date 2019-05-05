using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Domain.Models.States
{
    /* 4.2.2
     * The Model Publication state is a composite state that controls parameters
     * of messages that are published by a model.
     * The state includes a Publish Address, a Publish Period, a Publish AppKey Index,
     * a Publish Friendship Credential Flag, a Publish TTL, a Publish Retransmission Count,
     * and a Publish Retransmit Interval Steps.
     * Within an element, each model has a separate instance of Model Publication state.
     * It is highly recommended that models defined by higher layer specifications
     * use instances of the Model Publication state to control the publishing of messages. 
     */
    public class ModelPublication
    {
        /// <summary>
        /// 4.2.2.5
        /// </summary>
        public byte PublishTTL { get; set; }

    }
}
