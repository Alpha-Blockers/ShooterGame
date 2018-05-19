using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class PlayerNamePacketTests
    {
        [TestMethod()]
        public void PlayerNameEncodeWithoutPlayer()
        {
            // Define variables
            PlayerNamePacket p1 = new PlayerNamePacket(null, "SomeName");
            string encodedString = p1.Encode(true);

            // Check result
            Assert.AreEqual("n:SomeName", encodedString, "PlayerName didn't encode correctly");
        }

        [TestMethod()]
        public void PlayerNameEncodeWithPlayer()
        {
            // Setup static player list
            Player.TerminatePlayers(); // Clear any data from previous tests
            Player.InitPlayers(4);

            // Define variables
            PlayerNamePacket p1 = new PlayerNamePacket(Player.GetByIndex(2), "SomeName");
            string encodedString = p1.Encode(true);

            // Check result
            Assert.AreEqual("n2:SomeName", encodedString, "PlayerName Name didn't encode correctly");
        }

        [TestMethod()]
        public void PlayerNameDecodeWithoutPlayer()
        {
            // Define variables
            PlayerNamePacket p1 = new PlayerNamePacket(null, "SomeName");
            string encodedString = p1.Encode(true);
            PlayerNamePacket p2 = PlayerNamePacket.Decode(encodedString);

            // Check result
            Assert.AreEqual(p1.Player, p2.Player, "Player is not equal");
            Assert.AreEqual(p1.Name, p2.Name, "Name is not equal");
        }

        [TestMethod()]
        public void PlayerNameDecodeWithPlayer()
        {
            // Setup static player list
            Player.TerminatePlayers(); // Clear any data from previous tests
            Player.InitPlayers(4);

            // Define variables
            PlayerNamePacket p1 = new PlayerNamePacket(Player.GetByIndex(2), "SomeName");
            string encodedString = p1.Encode(true);
            PlayerNamePacket p2 = PlayerNamePacket.Decode(encodedString);

            // Check result
            Assert.AreEqual(p1.Player, p2.Player, "Player is not equal");
            Assert.AreEqual(p1.Name, p2.Name, "Name is not equal");
        }
    }
}