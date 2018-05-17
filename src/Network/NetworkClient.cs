using System.Net;
using System.Net.Sockets;

namespace ShooterGame
{
    /// <summary>
    /// The network controller can be setup as either a host or client.
    /// It contains a list of network players, as well as the local player.
    /// </summary>
    public class NetworkClient : NetworkController
    {
        private PacketTransferBuffer _transferBuffer;

        /// <summary>
        /// Network client constructor.
        /// </summary>
        /// <param name="address">Address of the host.</param>
        /// <param name="port">Port number on which the host is listening for new clients.</param>
        public NetworkClient(string address, ushort port = DEFAULT_PORT)
        {
            // Get host address
            //IPAddress serverIP = Dns.GetHostEntry(address).AddressList[0];
            IPAddress serverIP = Dns.GetHostEntry((address.ToLower() == "localhost") ? Dns.GetHostName() : address).AddressList[0];

            // Get host end-point (needed for connection)
            IPEndPoint endPoint = new IPEndPoint(serverIP, port);

            // Create socket
            Socket socket = new Socket(serverIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to host
            socket.Connect(endPoint);

            // Create packet transfer buffer
            _transferBuffer = new PacketTransferBuffer(socket);

            // Inform client server of client's name
            if ((Player.LocalPlayerName != null) && (Player.LocalPlayerName != ""))
                Send(new PlayerNamePacket(null, Player.LocalPlayerName));
        }

        /// <summary>
        /// Check if the local machine is the host of a network game.
        /// </summary>
        public override bool IsHost { get { return false; } }

        /// <summary>
        /// Send data via network.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        public override void Send(Packet data)
        {
            _transferBuffer.Enqueue(data.Encode(false));
        }

        /// <summary>
        /// Private method to manage received messages.
        /// </summary>
        /// <param name="encodedMessage"></param>
        private void Receive(string encodedMessage)
        {
            switch (encodedMessage[0])
            {
                case (char)PacketIdentifier.Message:
                    {
                        ChatPacket temp = ChatPacket.Decode(encodedMessage);
                        MessageLog.Add(temp);
                        break;
                    }
                case (char)PacketIdentifier.PlayerName:
                    {
                        PlayerNamePacket temp = PlayerNamePacket.Decode(encodedMessage);
                        temp.Apply();
                        MessageLog.Add(temp.ToString());
                        break;
                    }
                case (char)PacketIdentifier.LocalPlayerIndex:
                    {
                        LocalPlayerIndexPacket temp = LocalPlayerIndexPacket.Decode(encodedMessage);
                        temp.Apply();
                        break;
                    }
                case (char)PacketIdentifier.Bye:
                    {
                        Shutdown();
                        break;
                    }
            }
        }

        /// <summary>
        /// Check for new messages and run general updates for the network.
        /// </summary>
        public override void Update()
        {
            // Run update
            _transferBuffer.Update();

            // Check for packets
            for (string packet = _transferBuffer.Dequeue(); packet != null; packet = _transferBuffer.Dequeue())
                if (packet.Length > 0)
                    Receive(packet);
        }

        /// <summary>
        /// Inform the network to begin shutdown.
        /// </summary>
        public override void Shutdown()
        {
            // Check if transfer-buffer exists
            if (_transferBuffer != null)
            {
                // Queue a bye-message for send
                _transferBuffer.Enqueue(new ByePacket(), false);

                // Try to flush any pending send-data
                _transferBuffer.TryToSendData();

                // Shutdown transfer buffer
                _transferBuffer.Shutdown();
                _transferBuffer = null;
            }
        }
    }
}
