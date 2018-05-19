using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShooterGame.Tests
{
    [TestClass()]
    public class AttackComponentTests
    {
        [TestMethod()]
        public void AttackComponentTest()
        {
            AttackComponent a = new AttackComponent(5);
            Assert.AreEqual(5, a.Damage, "Damage amount incorrect");
        }

        [TestMethod()]
        public void AttackTest()
        {
            HealthComponent h = new HealthComponent(100);
            AttackComponent a = new AttackComponent(10);
            a.Attack(h);
            Assert.AreEqual(90, h.Health, "Health after attack incorrect");
        }
    }
}