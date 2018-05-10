
namespace ShooterGame
{
    /// <summary>
    /// Network packet identifiers are single-byte command characters used to signal what type of packet is being sent.
    /// The command character may or may not be followed by some other data, depending on the type of command.
    /// </summary>
    enum PacketIdentifier
    {
        /// <summary>
        /// Chat message command-byte. Is followed by a string (the chat message).
        /// </summary>
        Message = 'm',

        /// <summary>
        /// Player name command-byte. Is followed by a string (the player name).
        /// </summary>
        PlayerName = 'n',

        /// <summary>
        /// Local player index command-byte. Is followed by a number (the new index of the local player).
        /// </summary>
        LocalPlayerIndex = 'i',

        /// <summary>
        /// Bye command-byte. No extra data follows this command, however the connection will be closed after this command.
        /// </summary>
        Bye = 'b'
    }
}
