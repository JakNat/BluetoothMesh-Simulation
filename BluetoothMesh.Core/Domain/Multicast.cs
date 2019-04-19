using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Core.Domain
{
    public class Multicast
    {
        public string GroupName { get; set; }
        public int GroupId { get; protected set; }
        public int NumberOfSubscribingNodes { get; set; }
        public bool GroupAlreadyIssued { get; set; }

        public Multicast(int groupId, string groupName)
        {
            GroupName = groupName;
            GroupId = groupId;
            NumberOfSubscribingNodes = 0;
            GroupAlreadyIssued = false;
        }
        public void IncrementNumberOfSubscribingNodes()
        {
            ++this.NumberOfSubscribingNodes;
        }
        public void DecrementNumberOfSubscribingNodes()
        {
            --this.NumberOfSubscribingNodes;
        }
    }
}
