using System;
using System.Net.Sockets;
using System.Collections.Generic;
using static System.Text.Encoding;

namespace ShooterGame
{
    /// <summary>
    /// A packet transfer buffer manages the sending and receiving of packets send via a network socket.
    /// </summary>
    class PacketTransferBuffer
    {
        private const int MAX_PACKET_SIZE = 100000;

        private Socket _socket;
        private Queue<byte[]> _toSend; // Should never be null
        private Queue<byte[]> _received; // Should never be null

        private byte[] _sendBuffer; // Should never be null
        private int _sendOffset;
        private int _sendAmount;

        private byte[] _recvBuffer; // Should never be null
        private int _recvOffset;
        private int _recvAmount;

        /// <summary>
        /// Packet transfer buffer constructor.
        /// </summary>
        /// <param name="socket">The socket which will be used to send and receive network packets.</param>
        public PacketTransferBuffer(Socket socket)
        {
            _socket = socket;
            _toSend = new Queue<byte[]>();
            _received = new Queue<byte[]>();
            _sendBuffer = new byte[MAX_PACKET_SIZE + 1];
            _sendOffset = 0;
            _sendAmount = 0;
            _recvBuffer = new byte[MAX_PACKET_SIZE + 1];
            _recvOffset = 0;
            _recvAmount = 0;
        }

        /// <summary>
        /// Close the underlying socket and release all resources.
        /// </summary>
        public void Shutdown()
        {
            // Shutdown and close socket
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();

            // Clear other data
            _toSend.Clear();
            _received.Clear();
            _toSend = null;
            _received = null;
            _sendBuffer = null;
            _recvBuffer = null;
        }

        /// <summary>
        /// Get the socket which is used for sending and receiving data.
        /// </summary>
        public Socket Socket { get => _socket; }

        /// <summary>
        /// Add a packet to the send queue, to be send via the network socket.
        /// </summary>
        /// <param name="packet">Packet to be sent via the network.</param>
        public void Enqueue(string packet)
        {
            byte[] temp = UTF8.GetBytes(packet);
            if (temp.Length > MAX_PACKET_SIZE) throw new System.ArgumentException("network packet too large to send");
            _toSend.Enqueue(temp);
            FillSendBuffer();
        }

        /// <summary>
        /// Add a packet to the send queue, to be send via the network socket.
        /// </summary>
        /// <param name="packet">Packet to be sent via the network.</param>
        /// <param name="includePlayerIndex">The player index will be included if this is true</param>
        public void Enqueue(Packet packet, bool includePlayerIndex)
        {
            Enqueue(packet.Encode(includePlayerIndex));
        }

        /// <summary>
        /// Get next packet which was received from the network socket.
        /// </summary>
        /// <returns>Returns the packet, or null if none are pending.</returns>
        public string Dequeue()
        {
            while (_received.Count > 0)
            {
                try
                {
                    return UTF8.GetString(_received.Dequeue());
                }
                catch { /* Do nothing on error. Just skip the packet. */ }
            }
            return null;
        }

        /// <summary>
        /// Run general updates for this class. Call this often.
        /// </summary>
        public void Update()
        {
            TryToSendData();
            CheckForReceivedData();
        }

        /// <summary>
        /// Send data over the network, if any is pending.
        /// </summary>
        public void TryToSendData()
        {
            // Loop while the socket is able to send data, and while there is data to send
            while ((_sendAmount > 0) && _socket.Poll(0, SelectMode.SelectWrite))
            {
                // Send data
                int sent =_socket.Send(_sendBuffer, _sendOffset, _sendAmount, SocketFlags.None);

                // Update the offset and amount remaining to be sent
                if (sent < _sendAmount)
                {
                    // Update send offset and amount
                    _sendOffset += sent;
                    _sendAmount -= sent;
                }
                else
                {
                    // Reset send offset and amount
                    _sendAmount = 0;
                    _sendOffset = 0;

                    // Check if more data is waiting
                    if (_toSend.Count > 0) FillSendBuffer();
                }
            }
        }

        /// <summary>
        /// Receive data from the network, if any is pending.
        /// </summary>
        public void CheckForReceivedData()
        {
            // Loop while the socket has pending data
            while (_socket.Poll(0, SelectMode.SelectRead))
            {
                // Get amount if remaining space in the buffer
                int spaceAvailable = _recvBuffer.Length - _recvOffset - _recvAmount;

                // Check if data should be moved forward in the buffer
                if (spaceAvailable < _recvOffset)
                {
                    Array.Copy(_recvBuffer, _recvOffset, _recvBuffer, 0, _recvAmount);
                    _recvOffset = 0;
                    spaceAvailable = _recvBuffer.Length - _recvAmount;
                }

                // Receive data
                int received = _socket.Receive(_recvBuffer, _recvOffset, spaceAvailable, SocketFlags.None);

                // Update received data amount
                _recvAmount += received;

                // Process received data
                ProcessReceiveBuffer();
            }
        }

        /// <summary>
        /// Take packets from the to-send queue and add them to the send buffer, if there is enough space.
        /// </summary>
        private void FillSendBuffer()
        {
            // Move data forward in buffer, if required
            if (_sendOffset > 0)
            {
                Array.Copy(_sendBuffer, _sendOffset, _sendBuffer, 0, _sendAmount);
                _sendOffset = 0;
            }

            // Loop while there are queued packets and enough space to add them
            while ((_toSend.Count > 0) && (_sendBuffer.Length >= (_sendAmount + _toSend.Peek().Length + 1)))
            {
                // Copy packet to send-buffer
                // The send-offset is zero because it was reset earlier in the method
                byte[] temp = _toSend.Dequeue();
                Array.Copy(temp, 0, _sendBuffer, _sendAmount, temp.Length);

                // Update send amount
                // The +1 is for the null-terminator
                _sendAmount += temp.Length + 1;

                // Add the end of packet null-terminator
                // The send-offset is zero because it was reset earlier in the method
                _sendBuffer[_sendAmount] = 0;
            }
        }

        /// <summary>
        /// Check the receive buffer for whole packets and add them to the received packet queue.
        /// </summary>
        private void ProcessReceiveBuffer()
        {
            // Get end-of-data index
            int endOfData = _recvOffset + _recvAmount;

            // Find next null-terminator
            for (int i = _recvOffset; i < endOfData; i++)
            {
                // Check if null-terminator at this location
                if (_recvBuffer[i] == 0)
                {
                    // Get packet size
                    // This does NOT include the null-terminator
                    int size = i - _recvOffset;

                    // Check for empty packet
                    if (size > 0)
                    {
                        // Create new buffer
                        byte[] temp = new byte[size];

                        // Copy packet to buffer
                        Array.Copy(_recvBuffer, _recvOffset, temp, 0, size);

                        // Add to received packets queue
                        _received.Enqueue(temp);
                    }

                    // Increment offset
                    _recvOffset += size + 1;
                    _recvAmount -= size + 1;
                }
            }

            // Check if buffer is empty
            if (_recvAmount < 1)
            {
                _recvAmount = 0;
                _recvOffset = 0;
            }
        }
    }
}
