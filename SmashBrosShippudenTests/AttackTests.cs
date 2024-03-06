using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmashBrosShippuden;

namespace SmashBrosShippuden.Tests
{
    [TestClass()]
    public class AttackTests
    {
        [TestMethod()]
        public void getKnockbackTest()
        {
            // test that knockback is correctly returned from Attack class
            Attack attack = new Attack("Knuckles", AttackType.Special, "Right");
            Assert.AreEqual(attack.getKnockback(3), 2);
            Assert.AreEqual(attack.getKnockback(5), -2);
            Assert.AreEqual(attack.getKnockback(7), 2);

            attack = new Attack("Knuckles", AttackType.Special, "Left");
            Assert.AreEqual(attack.getKnockback(3), -2);
            Assert.AreEqual(attack.getKnockback(5), 2);
            Assert.AreEqual(attack.getKnockback(7), -2);
        }
    }
}