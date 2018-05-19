using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShooterGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class PositionComponentTests
    {
        [TestMethod()]
        public void PositionComponentTest()
        {
            Map map = new Map(1, 1);
            PositionComponent p = new PositionComponent(map, 1, 2);
            Assert.AreEqual(1, p.X, "X value incorrect");
            Assert.AreEqual(2, p.Y, "Y value incorrect");
            Assert.AreEqual(map.ChunkByIndex(0, 0), p.Chunk, "Chunk incorrect");
        }

        [TestMethod()]
        public void InternalDestroyTest()
        {
            Map map = new Map(1, 1);
            PositionComponent p = new PositionComponent(map, 1, 2);
            p.InternalDestroy();
            Assert.AreEqual(null, p.Chunk);
        }
    }
}