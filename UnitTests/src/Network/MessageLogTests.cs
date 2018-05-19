using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class MessageLogTests
    {
        [TestMethod()]
        public void ClearTest()
        {
            // Add dummy packet
            ChatPacket p = new ChatPacket(null, "message");
            MessageLog.Add(p);

            // Clear queue
            MessageLog.Clear();

            // Check result
            Assert.AreEqual(MessageLog.Count, 0, "Message count mismatch");
        }

        [TestMethod()]
        public void AddTest()
        {
            // Clear queue to make sure its empty (static data)
            MessageLog.Clear();

            // Add dummy packet
            ChatPacket p = new ChatPacket(null, "message");
            MessageLog.Add(p);

            // Check result
            Assert.AreEqual(MessageLog.Count, 1, "Message count mismatch");
            Assert.AreEqual(MessageLog.Messages.Last(), p, "Added message not at end of list");
        }

        [TestMethod()]
        public void AddLimit()
        {
            // Clear queue to make sure its empty (static data)
            MessageLog.Clear();

            // Add lots of dummy packets
            ChatPacket p = new ChatPacket(null, "message");
            for (int i = 0; i <= (MessageLog.MAX_MESSAGES + 2); i++)
                MessageLog.Add(p);

            // Check result
            Assert.AreEqual(MessageLog.Count, MessageLog.MAX_MESSAGES, "Message count mismatch");
        }

        [TestMethod()]
        public void MessageRollover()
        {
            // Clear queue to make sure its empty (static data)
            MessageLog.Clear();

            // Add lots of dummy packets
            ChatPacket p = new ChatPacket(null, "message");
            while (MessageLog.Count < MessageLog.MAX_MESSAGES)
                MessageLog.Add(p);

            // Add target message
            ChatPacket t = new ChatPacket(null, "target");
            MessageLog.Add(t);

            // Add another dummy packet
            MessageLog.Add(p);

            // Check result
            Assert.AreEqual(MessageLog.Messages[MessageLog.Count - 2], t, "Message count mismatch");
        }
    }
}