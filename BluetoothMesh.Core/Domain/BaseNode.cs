using System.Collections.Generic;
using BluetoothMesh.Core.Domain.Requests;
using System.Timers;

namespace BluetoothMesh.Core.Domain
{
    public enum Function { friend, low_energy, relay, proxy }

    public class Node
    {
        /// <summary>
        /// traktujemy node id jako jego numer portu
        /// zakres 0 - 65535
        /// </summary>
        public int Id { get; protected set; }
        public Listener Listener { get; set; }
        public List<Listener> SubscribtionsListeners { get; set; } = new List<Listener>();
        public double Range { get; set; } = 10;
        public Posistion Posistion { get; set; }
        public List<Multicast> Subscribed { get; set; } = new List<Multicast>();

        public Function Function { get; set; }
        public List<int> FriendNodes { get; set; }
        public List<BaseRequest> MessagesForLowPowerNodes { get; set; }
        public int PingFrequency { get; set; }

        public Node(int nodeId, Posistion posistion, Function function, params Multicast[] multicasts)
        {
            SetNodeId(nodeId);
            Posistion = posistion;
            Listener = new Listener(nodeId);
            Function = function;
            
            for (int i = 0; i<multicasts.Length; i++)
            {
                 Subscribed.Add(multicasts[i]);
                 if (!multicasts[i].GroupAlreadyIssued)
                 {
                     SubscribtionsListeners.Add(new Listener(multicasts[i].GroupId));
                     multicasts[i].GroupAlreadyIssued = true;
                 }
                 multicasts[i].IncrementNumberOfSubscribingNodes();
            }
            NodeFunctionSetter();
        }

        private void NodeFunctionSetter()
        {
            switch (this.Function)
            {
                case Function.friend:
                    this.FriendNodes = new List<int>();
                    this.MessagesForLowPowerNodes = new List<BaseRequest>();
                    break;
                case Function.low_energy:
                    this.FriendNodes = new List<int>();
                    this.PingFrequency = 5000;
                    break;
            }
        }

        public void SetParentNode(Node parentNodeId)
        {
            this.FriendNodes.Add(parentNodeId.Id);
            parentNodeId.FriendNodes.Add(this.Id);
        }

        public Node(int nodeId, double x, double y)
        {
            SetNodeId(nodeId);
            Posistion = new Posistion(x, y);
            Listener = new Listener(nodeId);
        }

        public void SetNodeId(int nodeId)
        {
            if (Id < 0 || Id > 65535)
            {
                throw new System.ArgumentException("NodeId must be between 0-65535");
            }
            Id = nodeId;
        }


        /*
        private void TimerSetter()
        {
            Timer timer = new Timer();
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(PingParentNode);
            timer.Start();
        }

        private static void PingParentNode(object sender, ElapsedEventArgs e)
        {
            BaseRequest pingRequest = new BaseRequest()
            {
                Heartbeats = 8,
                Message = "daj mi te dane człowieku",
                TargetNodeId = 2
            };
        }
        */
    }
}
