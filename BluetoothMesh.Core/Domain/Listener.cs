using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.UDP;
using NetworkCommsDotNet.Tools;
using System.Linq;
using System.Net;

namespace BluetoothMesh.Core.Domain
{
    /// <summary>
    /// Listener do odbierania wiadomośći 
    /// Jest tu używana biblioteka NetworkComs
    /// -> jak wysyłać własne obiekty http://www.networkcomms.net/custom-objects/
    /// </summary>
    public class Listener
    {
        public int Port { get; set; }
        /// <summary>
        /// Nasłuchiwanie według protokołu UDP ( do broadcastów)
        /// </summary>
        public UDPConnectionListener UDPConnectionListener{ get; set; }

        /// <summary>
        /// Pierwszy lepszy local ip
        /// </summary>
        public long Ip { get; set; } = HostInfo.IP.FilteredLocalAddresses().FirstOrDefault().Address;

        public Listener(int port)
        {
            Port = port;

            //Inicjalizacja listenera z podstawową konfiguracją
            UDPConnectionListener = new UDPConnectionListener
                (NetworkComms.DefaultSendReceiveOptions, ApplicationLayerProtocolStatus.Enabled, UDPConnection.DefaultUDPOptions);

            StartListening();
        }

        /// <summary>
        /// Rozpoczęcie pracy na danym ip i porcie
        /// </summary>
        public void StartListening()
        {
            Connection.StartListening(UDPConnectionListener, new IPEndPoint(Ip, Port));
        }

        public void StopListening()
        {
            Connection.StopListening(UDPConnectionListener);
        }

    }
}
