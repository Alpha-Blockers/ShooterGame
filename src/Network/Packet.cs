
namespace ShooterGame
{
    public abstract class Packet
    {
        protected const System.Char SEPARATOR = ':';

        /// <summary>
        /// Generate a string form of this class which can be sent using the SwinGame network functions.
        /// </summary>
        /// <param name="includePlayerIndex">The player index will be included if this is true</param>
        /// <returns>A string form of this class suitable for network transfer.</returns>
        public abstract string Encode(bool includePlayerIndex);

        /// <summary>
        /// Generate an encoded packet header string using only the packet identifier.
        /// </summary>
        /// <param name="identifier">Packet identifier to use.</param>
        /// <returns>A packet header string.</returns>
        protected static string EncodeHeader(PacketIdentifier identifier)
        {
            return ((char)identifier).ToString();
        }

        /// <summary>
        /// Generate an encoded packet header string using the packet identifier and player index.
        /// </summary>
        /// <param name="identifier">Packet identifier to use.</param>
        /// <param name="player">Player to include in the header, if not null.</param>
        /// <returns>A packet header string.</returns>
        protected static string EncodeHeader(PacketIdentifier identifier, Player player)
        {
            if (player != null)
                return EncodeHeader(identifier) + player.Index.ToString();
            else
                return EncodeHeader(identifier);
        }

        /// <summary>
        /// Generate an encoded packet string using the packet identifier and tail string.
        /// Form a complete encoded message using header info and a tail string.
        /// </summary>
        /// <param name="identifier">Packet identifier to use.</param>
        /// <param name="tail">Payload string to include in the packet.</param>
        /// <returns>A whole packet encoded-string.</returns>
        protected static string EncodePacket(PacketIdentifier identifier, string tail)
        {
            return EncodeHeader(identifier) + SEPARATOR + tail;
        }

        /// <summary>
        /// Generate an encoded packet header string using the packet identifier and player index.
        /// </summary>
        /// <param name="identifier">Packet identifier to use.</param>
        /// <param name="player">Player to include in the header, if not null.</param>
        /// <param name="tail">Payload string to include in the packet.</param>
        /// <returns>A whole packet encoded-string.</returns>
        protected static string EncodePacket(PacketIdentifier identifier, Player player, string tail)
        {
            return EncodeHeader(identifier, player) + SEPARATOR + tail;
        }

        /// <summary>
        /// Check that the packet begins with the correct packet identifier.
        /// Throws an exception if the wrong header is found.
        /// </summary>
        /// <param name="encodedString">Encoded packet string.</param>
        /// <param name="header">Header which should be present.</param>
        protected static void VerifyIdentifier(string encodedString, PacketIdentifier header)
        {
            if (encodedString[0] != (char)header)
                throw new System.ArgumentException("expected message to begin with '" + EncodeHeader(header) + "'");
        }

        /// <summary>
        /// Split an encoded packet string into its components and return the tail.
        /// </summary>
        /// <param name="encodedString">Source encoded packet string.</param>
        /// <returns>Returns a string containing whatever data was after the separator.</returns>
        protected static string DecodeTail(string encodedString)
        {
            // Find separator between header data and message body
            int i = encodedString.IndexOf(SEPARATOR);
            if (i < 1)
                return null;

            // Return packet tail string
            return encodedString.Substring(i + 1);
        }

        /// <summary>
        /// Split an encoded packet string into its components and return the tail.
        /// </summary>
        /// <param name="encodedString">Source encoded packet string.</param>
        /// <param name="playerOverride">If not null, this value is used to override the player listed by the packet.</param>
        /// <param name="player">Returns the player to whom the packet relates.</param>
        /// <returns>Returns a string containing whatever data was after the separator.</returns>
        protected static string DecodeTail(string encodedString, Player playerOverride, out Player player)
        {
            // Find separator between header data and message body
            int i = encodedString.IndexOf(SEPARATOR);
            if (i < 1)
            {
                player = playerOverride;
                return null;
            }

            // Record player who sent the message
            if (playerOverride != null)
            {
                player = playerOverride;
            }
            else
            {
                // Get string with player index (skip over the header byte)
                string playerIndexString = encodedString.Substring(1, i - 1);

                // Get player index
                int playerIndex = -1;
                if (playerIndexString.Length > 0)
                {
                    try { playerIndex = int.Parse(playerIndexString); } catch { }
                }

                // Get player
                // This will return null if the index is invalid
                player = Player.GetByIndex(playerIndex);
            }

            // Return packet tail string
            return encodedString.Substring(i + 1);
        }
    }
}
