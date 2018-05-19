using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShooterGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class MovementComponentTests
    {
        [TestMethod()]
        public void MovementComponentTest()
        {
            MovementComponent c = new MovementComponent();
            Assert.AreEqual(0, c.X, "X value incorrect");
            Assert.AreEqual(0, c.Y, "Y value incorrect");
        }

        [TestMethod()]
        public void MovementComponentTest1()
        {
            MovementComponent c = new MovementComponent(2, 3);
            Assert.AreEqual(2, c.X, "X value incorrect");
            Assert.AreEqual(3, c.Y, "Y value incorrect");
        }

        [TestMethod()]
        public void InternalDestroyTest()
        {
            MovementComponent c = new MovementComponent(2, 3);
            c.InternalDestroy();
            Assert.AreEqual(0, c.X, "X value incorrect");
            Assert.AreEqual(0, c.Y, "Y value incorrect");
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Map map = new Map(1, 1);
            PositionComponent p = new PositionComponent(map, 0, 0);
            MovementComponent m = new MovementComponent(2, 3);
            Entity e = new Entity { Position = p, Movement = m };
            e.Movement.Update();
            Assert.AreEqual(2, p.X, "X value incorrect");
            Assert.AreEqual(3, p.Y, "Y value incorrect");
        }
    }
}