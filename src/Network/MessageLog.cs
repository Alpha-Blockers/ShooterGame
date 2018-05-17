using System.Collections.Generic;

namespace ShooterGame
{
    public static class MessageLog
    {
        public const int MAX_MESSAGES = 20;

        private static List<ChatPacket> _messages = new List<ChatPacket>();

        /// <summary>
        /// Get received message list
        /// </summary>
        public static List<ChatPacket> Messages { get => _messages; }

        /// <summary>
        /// Add a message to the local message list. Does not relay to network in any form.
        /// </summary>
        /// <param name="m">The message to add</param>
        public static void Add(ChatPacket m)
        {
            // Remove a message if the maximum number of messages has been reached
            if (_messages.Count > MAX_MESSAGES)
                _messages.RemoveAt(0);

            // Add new message
            _messages.Add(m);
        }

        /// <summary>
        /// Add a message to the local message list. Does not relay to network in any form.
        /// This version of add is intended for debugging only.
        /// </summary>
        /// <param name="s">String to be added to the message list.</param>
        public static void Add(string s)
        {
            Add(new ChatPacket(null, s));
        }
    }
}
