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
            // Create test log
            MessageLog log = new MessageLog();

            // Add dummy packet
            ChatPacket p = new ChatPacket(null, "message");
            log.Add(p);

            // Clear queue
            log.Clear();

            // Check result
            Assert.AreEqual(log.Count, 0, "Message count mismatch");
        }

        [TestMethod()]
        public void AddTest()
        {
            // Create test log
            MessageLog log = new MessageLog();

            // Add dummy packet
            ChatPacket p = new ChatPacket(null, "message");
            log.Add(p);

            // Check result
            Assert.AreEqual(log.Count, 1, "Message count mismatch");
            Assert.AreEqual(log.Messages.Last(), p, "Added message not at end of list");
        }

        [TestMethod()]
        public void AddLimit()
        {
            // Create test log
            MessageLog log = new MessageLog();

            // Add lots of dummy packets
            ChatPacket p = new ChatPacket(null, "message");
            for (int i = 0; i <= (MessageLog.CountMax + 2); i++)
                log.Add(p);

            // Check result
            Assert.AreEqual(log.Count, MessageLog.CountMax, "Message count mismatch");
        }

        [TestMethod()]
        public void MessageRollover()
        {
            // Create test log
            MessageLog log = new MessageLog();

            // Add lots of dummy packets
            ChatPacket p = new ChatPacket(null, "message");
            while (log.Count < MessageLog.CountMax)
                log.Add(p);

            // Add target message
            ChatPacket t = new ChatPacket(null, "target");
            log.Add(t);

            // Add another dummy packet
            log.Add(p);

            // Check result
            Assert.AreEqual(log.Messages[log.Count - 2], t, "Message count mismatch");
        }
    }
}