using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShooterGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class HealthComponentTests
    {
        [TestMethod()]
        public void HealthComponentTest()
        {
            HealthComponent c = new HealthComponent(10);
            Assert.AreEqual(10, c.Health, "Health value incorrect");
        }
    }
}