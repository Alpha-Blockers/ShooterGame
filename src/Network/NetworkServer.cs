using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ShooterGame
{
    public class NetworkServer : NetworkController
    {
        private List<InternalClient> _client;
        private Socket _listener;

        /// <summary>
        /// Internal client data.
        /// </summary>
        private class InternalClient
        {
            public PacketTransferBuffer _transferBuffer;
            public Player _player;

            /// <summary>
            /// Constructor for the server interal client data.
            /// </summary>
            /// <param name="socket">The socket which connects to the client.</param>
            public InternalClient(Socket socket)
            {
                _transferBuffer = new PacketTransferBuffer(socket);
                _player = Player.AllocatePlayer();
            }

            /// <summary>
            /// Access the packet transfer buffer which is used to send and receive data from this client.
            /// </summary>
            public PacketTransferBuffer TransferBuffer { get => _transferBuffer; }

            /// <summary>
            /// Get the player which is linked to this client, if any.
            /// </summary>
            public Player Player { get => _player; }

            /// <summary>
            /// Shutdown and clear all data linked to this client.
            /// </summary>
            public void Shutdown()
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

                // Null the player linkage
                _player = null;
            }
        }

        private void SetupListener(ushort port)
        {
            // Get local address
            IPAddress address = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            // Get local end-point (needed for connection)
            IPEndPoint endPoint = new IPEndPoint(address, port);

            // Create socket
            _listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind socket (link to listener port)
            _listener.Bind(endPoint);

            // Set socket to listen for connecting clients
            // Set 'backlog' to some arbitrary amount
            _listener.Listen(5);
        }

        /// <summary>
        /// Network server constructor.
        /// </summary>
        /// <param name="players">Number of players this game should support.</param>
        /// <param name="port">Port number on which to listen for connecting clients.</param>
        public NetworkServer(int players = 4, ushort port = DEFAULT_PORT)
        {
            // Stupidity check
            if (players < 1)
                throw new System.ArgumentException("value must be greater than zero", "players");

            // Create listener socket
            SetupListener(port);

            // Setup player list
            Player.InitPlayers(players);
            Player.SetLocalPlayerIndex(0);

            // Create client list
            _client = new List<InternalClient>();
        }

        /// <summary>
        /// Check if the local machine is the host of a network game.
        /// </summary>
        public override bool IsHost { get { return true; } }

        /// <summary>
        /// Send data to all network clients.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        public override void Send(Packet data)
        {
            string temp = data.Encode(true);
            foreach (InternalClient c in _client)
                c.TransferBuffer.Enqueue(temp);
        }

        /// <summary>
        /// Private method to manage received messages.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="encodedMessage"></param>
        private void Receive(InternalClient from, string encodedMessage)
        {
            switch (encodedMessage[0])
            {
                case (char)PacketIdentifier.Message:
                    {
                        ChatPacket temp = ChatPacket.Decode(encodedMessage, from.Player);
                        Send(temp);
                        MessageLog.Current?.Add(temp);
                        break;
                    }
                case (char)PacketIdentifier.PlayerName:
                    {
                        PlayerNamePacket temp = PlayerNamePacket.Decode(encodedMessage, from.Player);
                        Send(temp);
                        temp.Apply();
                        MessageLog.Current?.Add(temp.ToString());
                        break;
                    }
                case (char)PacketIdentifier.LocalPlayerIndex:
                    {
                        break; // Ignore these messages if from clients
                    }
                case (char)PacketIdentifier.Bye:
                    {
                        throw new System.NotImplementedException();
                    }
            }
        }

        /// <summary>
        /// Check for new messages and run general updates for the network.
        /// </summary>
        public override void Update()
        {
            // Loop through clients list
            // Run update methods and check for received packets
            foreach (InternalClient c in _client)
            {
                // Run update
                c.TransferBuffer.Update();

                // Check for packets
                for (string packet = c.TransferBuffer.Dequeue(); packet != null; packet = c.TransferBuffer.Dequeue())
                    if (packet.Length > 0)
                        Receive(c, packet);
            }

            // Check for new clients
            while (_listener.Poll(0, SelectMode.SelectRead))
            {
                // Accept the connection and create a socket
                Socket s = _listener.Accept();

                // Create a new internal-client
                InternalClient c = new InternalClient(s);

                // Check if client has a player
                // If not then the server is full
                if (c.Player != null)
                {
                    // Add client to client-list
                    _client.Add(c);

                    // Send welcome messages
                    c.TransferBuffer.Enqueue(new LocalPlayerIndexPacket(c.Player.Index), false);
                }
                else
                {
                    // Shutdown the client
                    // This will send a bye-message
                    c.Shutdown();
                }
            }
        }

        /// <summary>
        /// Inform the network to begin shutdown.
        /// </summary>
        public override void Shutdown()
        {
            // Check if client list exists
            if (_client != null)
            {
                // Loop through all clients and shutdown each down
                // This will send a bye-message to each as well
                foreach (InternalClient c in _client)
                    c.Shutdown();

                // Clear the client list
                _client.Clear();
            }
        }
    }
}
