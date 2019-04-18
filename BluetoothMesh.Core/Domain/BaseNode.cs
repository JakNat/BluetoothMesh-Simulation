using System.Collections.Generic;

namespace BluetoothMesh.Core.Domain
{
    public class BaseNode
    {
        /// <summary>
        /// traktujemy node id jako jego numer portu
        /// zakres 0 - 65535
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// listener do nasłuchiwania wysyłanych do niego wiadomości
        /// </summary>
        public Listener Listener { get; set; }

        /// <summary>
        /// zasięg
        /// </summary>
        public double Range { get; set; } = 10;
        public Posistion Posistion { get; set; }

        public BaseNode(int nodeId, Posistion posistion)
        {
            SetNodeId(nodeId);
            Posistion = posistion;
            Listener = new Listener(nodeId);
        }

        public BaseNode(int nodeId, double x, double y)
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

    }
}
