using BluetoothMesh.Core.Domain;
using BluetoothMesh.Core.Domain.Requests;
using BluetoothMesh.Infrastructure.Services;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMesh.Infrastructure.Configuration
{
    /// <summary>
    /// Klasa odwierciedla w pełni samodzielnego noda :v
    /// </summary>
    public class NodeServer
    {
        public BaseNode Node { get; set; }

        /// <summary>
        /// id otrzymanych już wiadomośći
        /// </summary>
        public List<Guid> ReceivedRequests { get; set; } = new List<Guid>();

        /// <summary>
        /// Serwis do komunikacji
        /// </summary>
        private IBroadcastService _broadcastService;

        /// <summary>
        /// konstruktor bez zdefiniowanego serwisu
        /// jeśli chcemy określić go później
        /// </summary>
        public NodeServer(BaseNode node)
        {
            Node = node;
        }
        public NodeServer(BaseNode node, IBroadcastService broadcastService)
        {
            Node = node;
            _broadcastService = broadcastService;
        }

        /// <summary>
        /// Podstawowa metoda do wysłania wiadomości
        /// </summary>
        public void Send<T>(T request) where T : BaseRequest
        {
            request.BroadCastingNodeId = Node.Id;
            _broadcastService.SendBroadcast(Node, request);
        }

        /// <summary>
        /// Dodajemy do naszego serwera rodzaj wiadomości którą możemy obsłużyc jeśli ktoś do nas takową 
        /// w tym przypadku rejestrujemy nasłuchiwanie wiadomości o tytule "BasicMessage" który wysyła obiekty typu <BaseRequest>
        /// </summary>
        public void RegisterBasicResponse()
        { 

            //<BaseRequest> => jakiego typu jest wiadomość
            //"BasicMessage" => nazwa wiadomosci ( powinna być klasa statyczna lub enumy by przechowywać je wszystkie)
            //BasicResponse => nazwa metody która się wywoła 
            Node.Listener.UDPConnectionListener.AppendIncomingPacketHandler<BaseRequest>("BasicMessage", BasicResponse);
        }

        public void SetBroadCastService(IBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }

        /// <summary>
        /// Jest to metoda która wywołuje się gdy nasz węzeł dostanie wiadomość
        /// wersja beta obsługi BasicMessasge
        /// </summary>
        /// <param name="packetHeader">Nagłówek pakietu powiązany z wiadomością przychodzącą</param>
        /// <param name="connection">Połączenie przychodzącej wiadomości</param>
        /// <param name="incomingObject">Wiadomość którą otrzymaliśmy</param>
        public void BasicResponse(PacketHeader packetHeader, Connection connection, BaseRequest incomingObject)
        {
            //to do
            //if (!Node.SubsribedNodes.Contains(incomingObject.BroadCastingNodeId))
            //{
            //    return;
            //}

            if (ReceivedRequests.Contains(incomingObject.RequestId))
            {
                Console.WriteLine("Node nr " + Node.Id + " blocked from nr " + incomingObject.BroadCastingNodeId);
                return;
            }

            ReceivedRequests.Add(incomingObject.RequestId);

            Console.WriteLine("Node nr " + Node.Id + " get message from {Node " + incomingObject.BroadCastingNodeId + "}");
            if (incomingObject.TargetNodeId == Node.Id)
            {
                Console.WriteLine("CONGRATS!!");
                return;
            }

            if (incomingObject.Heartbeats == 0)
            {
                Console.WriteLine("Message died :<");
                return;
            }

            var newRequest = incomingObject;
            newRequest.Heartbeats--;
            newRequest.BroadCastingNodeId = Node.Id;
            _broadcastService.SendBroadcast(Node, newRequest);
        }
    }
}
