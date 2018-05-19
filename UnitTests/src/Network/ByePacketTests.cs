using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class ByePacketTests
    {
        [TestMethod()]
        public void ByeEncode()
        {
            // Define variables
            ByePacket p1 = new ByePacket();
            string encodedString = p1.Encode(true);

            // Check result
            Assert.AreEqual("b", encodedString, "Bye message didn't encode correctly");
        }

        [TestMethod()]
        public void ByeDecode()
        {
            // Define variables
            ByePacket p1 = new ByePacket();
            string encodedString = p1.Encode(true);

            // Try to decode packet
            try
            {
                ByePacket p2 = ByePacket.Decode(encodedString);
            }
            catch(System.Exception e)
            {
                Assert.Fail("Bye packet exception: " + e.Message);
            }
        }
    }
}