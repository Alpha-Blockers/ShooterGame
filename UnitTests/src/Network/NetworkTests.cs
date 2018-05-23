using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class NetworkTests
    {
        [TestMethod()]
        public void ConnectLocalhost()
        {
            // Clear any static data which may exist
            Player.TerminatePlayers();
            if (MessageLog.Current != null)
                MessageLog.Current.Clear();
            else
                MessageLog.Current = new MessageLog();

            // Define variables
            NetworkServer server = new NetworkServer();
            NetworkClient client = new NetworkClient("localhost");

            // Send message
            client.Send(new ChatPacket(null, "test"));

            // Run updates a few times
            for (int i = 0; i < 5; i++)
            {
                client.Update();
                server.Update();
            }

            // Check for message
            Assert.AreEqual("test", MessageLog.Current.Messages.Last().Message, "Client message not received");
        }
    }
}