
namespace ShooterGame
{
    interface IPacket
    {
        /// <summary>
        /// Generate a string form of this class which can be sent using the SwinGame network functions.
        /// </summary>
        /// <param name="includePlayerIndex">The player index will be included if this is true</param>
        /// <returns>A string form of this class suitable for network transfer.</returns>
        string Encode(bool includePlayerIndex);
    }
}
