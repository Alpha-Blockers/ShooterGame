using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class ChatPacketTests
    {

        [TestMethod()]
        public void ChatEncodeWithoutPlayer()
        {
            // Define variables
            ChatPacket p1 = new ChatPacket(null, "hello");
            string encodedString = p1.Encode(true);

            // Check result
            Assert.AreEqual("m:hello", encodedString, "Chat message didn't encode correctly");
        }

        [TestMethod()]
        public void ChatEncodeWithPlayer()
        {
            // Setup static player list
            Player.TerminatePlayers(); // Clear any data from previous tests
            Player.InitPlayers(4);

            // Define variables
            ChatPacket p1 = new ChatPacket(Player.GetByIndex(2), "hello");
            string encodedString = p1.Encode(true);

            // Check result
            Assert.AreEqual("m2:hello", encodedString, "Chat message didn't encode correctly");
        }

        [TestMethod()]
        public void ChatDecodeWithoutPlayer()
        {
            // Define variables
            ChatPacket p1 = new ChatPacket(null, "hello");
            string encodedString = p1.Encode(true);
            ChatPacket p2 = ChatPacket.Decode(encodedString);

            // Check result
            Assert.AreEqual(p1.Player, p2.Player, "Player is not equal");
            Assert.AreEqual(p1.Message, p2.Message, "Message is not equal");
        }

        [TestMethod()]
        public void ChatDecodeWithPlayer()
        {
            // Setup static player list
            Player.TerminatePlayers(); // Clear any data from previous tests
            Player.InitPlayers(4);

            // Define variables
            ChatPacket p1 = new ChatPacket(Player.GetByIndex(2), "hello");
            string encodedString = p1.Encode(true);
            ChatPacket p2 = ChatPacket.Decode(encodedString);

            // Check result
            Assert.AreEqual(p1.Player, p2.Player, "Player is not equal");
            Assert.AreEqual(p1.Message, p2.Message, "Message is not equal");
        }
    }
}