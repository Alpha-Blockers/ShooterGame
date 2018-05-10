
namespace ShooterGame
{
    abstract class NetworkController
    {
        public const ushort DEFAULT_PORT = 8000;

        private static NetworkController _current;

        /// <summary>
        /// Get or set current active network controller.
        /// </summary>
        public static NetworkController Current
        {
            get => _current;
            set
            {
                if (_current != value)
                {
                    _current?.Shutdown();
                    _current = value;
                }
            }
        }

        /// <summary>
        /// Check if the local machine is the host of a network game.
        /// </summary>
        public abstract bool IsHost { get; }

        /// <summary>
        /// Send data via network.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        public abstract void Send(Packet data);

        /// <summary>
        /// Check for new messages and run general updates for the network.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Inform the network to begin shutdown.
        /// </summary>
        public abstract void Shutdown();
    }
}
